<template>
  <div class="content-table" id="table-view" ref="tableView">
    <table :class="['table-' + type.toLowerCase(), { loading: isLoading }]">
      <thead>
        <tr>
          <th></th>
          <th>
            <span
              class="checkbox-container"
              @dblclick.stop=""
              @click="selectAll"
            >
              <input type="checkbox" name="delete" v-model="isSelectAll" />
              <div class="checkmark"></div>
            </span>
          </th>
          <th v-for="(header, i) in headers" :key="i">
            <span>{{ header.name }}</span>
          </th>
          <th><span>CHỨC NĂNG</span></th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(e, index) in isLoading ? loadingRows : employees"
          :key="index"
          @dblclick="$emit('rowDblClick', e[type + 'Id'])"
          @click="rowClickHandle(e, type)"
          :class="{ selected: checkSelected(e, type) }"
        >
          <td @click.stop="" @dblclick.stop=""></td>
          <td>
            <span class="checkbox-container" @dblclick.stop="">
              <input type="checkbox" name="delete" v-model="e.isSelected" />
              <div class="checkmark"></div>
            </span>
          </td>
          <td :title="e[cell.key]" v-for="(cell, k) in headers" :key="k">
            <span v-if="cell.key == 'DateOfBirth'">
              {{ dateFormatVer2(e[cell.key], "dd/mm/yyyy") }}</span
            >
            <span v-else-if="cell.key.split('Is')[1] != null">
              <span class="checkbox-container disable">
                <input type="checkbox" name="delete" v-model="e[cell.key]" />
                <div class="checkmark"></div>
              </span>
            </span>
            <span v-else> {{ e[cell.key] }} </span>
          </td>
          <td @dblclick.stop="">
            <div class="tools">
              <span
                class="update-btn"
                @click.stop="$emit('rowDblClick', e[type + 'Id'])"
              >
                Sửa
              </span>
              <span
                class="more-action-btn"
                @click.stop="moreAction($event, e)"
              ></span>
            </div>
          </td>
          <td @dblclick.stop=""></td>
        </tr>
      </tbody>
    </table>
    <div class="dropdown-data" entityid="" entitycode="" id="drop-options">
      <div class="dropdown-item" @click="cloneOptionClicked">
        <div class="item-text">Nhân bản</div>
      </div>
      <div class="dropdown-item" @click="deleteOptionClicked">
        <div class="item-text">Xóa</div>
      </div>
      <div class="dropdown-item">
        <div class="item-text">Sử dụng</div>
      </div>
    </div>
  </div>
</template>

<script>
import EventBus from "../../event-bus/EventBus";
import axios from "axios";
import ultis from "../../mixins/ultis";

export default {
  name: "BaseTable",
  components: {},
  mixins: [ultis],
  props: {
    tableKey: {},
    // Kiểu entity: employee hoặc customer, ...
    type: {
      type: String, // ex: type="Employee"
      required: true,
    },
    //Mảng tên header <th>
    headers: {
      type: Array,
      required: true,
    },
    api: {
      type: String,
      require: false,
    },
    loading: {
      type: Boolean,
      required: false,
    },
  },
  data() {
    return {
      employees: null,
      isSelectAll: false,
      //map từ tên type với cách gọi tên TViet
      typeMap: {
        Employee: "Nhân Viên",
        Customer: "Khách Hàng",
      },
      loadingRows: [{}, {}, {}],
      isLoading: false,
      tableScrollTop: 0,
      optionsPopup: null,
      curRow: null,
    };
  },
  updated: function () {
    /**
     * Giữ trạng thái selected của những dòng đã click chọn khi refresh table
     * CreatedBy: HungNguyen81 (18-08-2021)
     */
    this.$nextTick(function () {
      // Code that will run only after the
      // entire view has been re-rendered
      if (this.employees) {
        for (let e of this.employees) {
          let stored = localStorage.getItem("select");
          if (stored) stored = JSON.parse(stored);
          else return;
          if (!stored.includes(e[this.type + "Code"])) {
            this.isSelectAll = false;
            return;
          }
        }
        this.isSelectAll = true;
      }
    });
  },
  mounted() {
    this.optionsPopup = document.getElementById("drop-options");

    // lắng nghe sự kiện click ra ngoài => ẩn dropdown list
    EventBus.$on("appClick", (target) => {
      if (!target.classList.contains("dropdown-item")) {
        this.hideMoreActionDrop();
      }
    });
    // thay đổi vị trí menu options popup khi scroll
    EventBus.$on("scrollView", (top, left) => {
      this.setOptionsPopupPosition(top, left);
    });
  },
  computed: {},
  watch: {
    // Handle khi re-render bảng
    tableKey: function () {
      this.buildTableContent();
      this.hideMoreActionDrop();
    },

    // hiển thị loader khi đang tải dữ liệu
    loading: function (val) {
      this.isLoading = val;
    },
  },
  methods: {
    /**
     * Handle khi click nút chọn chức năng ở bảng
     * CreatedBy: HungNguyen81 (31-08-2021)
     */
    moreAction(event, data) {
      this.curRow = event.target;
      let contentBody = document.querySelector(".content-body");

      // xử lí toggle popup
      if (
        this.optionsPopup.getAttribute("entityId") == data[`${this.type}Id`] &&
        this.optionsPopup.style.visibility != "hidden"
      ) {
        this.hideMoreActionDrop();
        return;
      }

      // set vị trí cho popup
      this.setOptionsPopupPosition(
        contentBody.scrollTop,
        contentBody.scrollLeft
      );

      this.optionsPopup.style.visibility = "visible";

      // set data để truyền khi click
      this.optionsPopup.setAttribute("entityid", data[this.type + "Id"]);
      this.optionsPopup.setAttribute("entitycode", data[this.type + "Code"]);
    },

    /**
     * Set vị trí cho options popup
     * CreatedBy: HungNguyen81 (31-08-2021)
     */
    setOptionsPopupPosition(top, left) {
      let offsetX = -99;
      let offsetY = -288;
      if(document.querySelector('.menu').classList.contains('collapse')){
        offsetX = -99;
        offsetY = -162;
      }

      if (this.curRow) {
        var pos = this.curRow.getBoundingClientRect();
        this.optionsPopup.style.top = pos.top + offsetX + top + "px";
        this.optionsPopup.style.left = pos.left + offsetY + left + "px";
      } else return;
    },

    /**
     * Ẩn dropdown chức năng
     * CreatedBy: HungNguyen81 (29-08-2021)
     */
    hideMoreActionDrop() {
      var options = document.getElementById("drop-options");
      options.style.visibility = "hidden";
    },

    /**
     * Handle khi click "nhân bản"
     * CreatedBy: HungNguyen81 (29-08-2021)
     */
    cloneOptionClicked(e) {
      var optionsDrop = e.target.parentNode;
      var id = optionsDrop.getAttribute("entityId");
      this.$emit("cloneEntity", id);
    },

    /**
     * Handle khi click option "xóa"
     * CreatedBy: HungNguyen81 (29-08-2021)
     */
    deleteOptionClicked(e) {
      var optionsDrop = e.target.parentNode;
      var id = optionsDrop.getAttribute("entityid");
      var code = optionsDrop.getAttribute("entitycode");
      this.$emit("deleteEntity", id, code);
    },

    /**
     * Lấy dữ liệu từ API và đổ vào table
     *  CreatedBy: HungNguyen81 (18-08-2021)
     */
    buildTableContent() {
      this.isLoading = true;

      if (this.api) {
        axios
          .get(this.api)
          .then((res) => {
            this.isLoading = false;
            this.employees = res.data.Data;

            if (this.employees) {
              this.$emit("dataLoaded");
              this.$emit(
                "getPagingInfo",
                res.data.TotalPage,
                res.data.TotalRecord
              );
              this.employees = this.employees.map((e) => ({
                ...e,
                isSelected: false,
              }));
            } else {
              this.$emit(
                "showToast",
                "warning",
                this.$resourceVn.NotFoundTitle,
                this.$resourceVn.EmployeeNotFoundMsg
              );
              this.$emit("dataLoaded");
            }
          })
          .catch((err) => {
            this.isLoading = false;

            // Handle khi không có kết nối Internet
            if (!err.response) {
              this.$emit("dataLoaded");
              this.$emit(
                "showToast",
                "error",
                this.$resourceVn.ErrorTitle,
                this.$resourceVn.NetworkErrorMsg
              );
              return;
            }
            
            this.$emit("dataLoaded");
            this.$emit(
              "showToast",
              "error",
              this.$resourceVn.ErrorTitle,
              this.$resourceVn.EmployeeNotFoundMsg
            );
          });
      }
    },

    /**
     * Handle khi click chuột vào table row
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    rowClickHandle(e, type) {
      this.$emit("rowClick", e[type + "Id"], e[type + "Code"], e["FullName"]);
      e.isSelected = !e.isSelected;
      let stored = localStorage.getItem("select");

      if (stored) {
        stored = JSON.parse(stored);
        if (e.isSelected) {
          stored.push(e[type + "Code"]);
        } else {
          let code = e[type + "Code"];
          let index = stored.indexOf(code);
          stored.splice(index, 1);
        }
        localStorage.setItem("select", JSON.stringify(stored));
      } else {
        localStorage.setItem(
          "select",
          JSON.stringify([].push(e[type + "Code"]))
        );
      }
    },

    /**
     * Check các dòng được chọn (selected)
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    checkSelected(e) {
      let code = e[this.type + "Code"];
      let stored = localStorage["select"];

      if (stored) {
        e.isSelected = JSON.parse(stored).includes(code);
        return e.isSelected;
      }
    },

    /**
     * Chọn tất cả dòng trong 1 trang của bảng <click checkbox chọn tất cả>
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    selectAll() {
      this.isSelectAll = !this.isSelectAll;
      let list = localStorage.getItem("select");
      try {
        list = JSON.parse(list);
      } catch (err) {
        list = [];
        localStorage.setItem("select", JSON.stringify([]));
      }

      for (let e of this.employees) {
        let code = e[this.type + "Code"];
        if (this.isSelectAll) {
          if (!list.includes(code)) {
            list.push(code);
            this.$emit("rowClick", e[this.type + "Id"], code, e["FullName"]);
          }
        } else {
          let index = list.indexOf(code);
          list.splice(index, 1);
          this.$emit("rowClick", e[this.type + "Id"], code, e["FullName"]);
        }
      }
      localStorage.setItem("select", JSON.stringify(list));
    },
  },
};
</script>

<style scoped>
@import "../../css/base/table.css";
@import "../../css/base/checkbox.css";
</style>