<template>
  <div>
    <div class="content">
      <div class="content-heading">
        <b class="title">{{ entityMap[entityName] }}</b>
        <div class="buttons-containner">
          <BaseButtonIcon
            :value="$resourceVn[`${entityName}ButtonDeleteText`]"
            type="button-delete"
            icon="icon-delete"
            :onclick="delBtnClick"
            :class="{ 'hidden': !delBtnActive }"
          ></BaseButtonIcon>
          <BaseButtonIcon
            :value="$resourceVn[`${entityName}ButtonAddText`]"
            icon="icon-add"
            :onclick="btnAddClick"
          ></BaseButtonIcon>
        </div>
      </div>

      <div class="content-body" @scroll="handleScroll">
        <div class="content-search">
          <div class="more-feature-btn-wrap">
            <input
              type="text"
              class="textbox-default search-field"
              :placeholder="$resourceVn[`${entityName}SearchPlaceHolderText`]"
              v-model="searchInput"
            />
            <div class="search-icon"></div>
            <div
              class="feature-btn btn-refresh"
              @click="refreshTableSelected"
            ></div>
            <div
              class="feature-btn"
              :class="btn.featureClass"
              @click="btn.action"
              v-for="(btn, i) in features"
              :key="i"
            ></div>
          </div>
        </div>

        <Table
          :type="entityName"
          :headers="headers"
          :api="`${
            $config.BASE_API
          }/${entityName}s/${entityName.toLowerCase()}Filter?pageSize=${pageSize}&pageNumber=${pageNumber}&filterString=${
            searchInput + getApiFilterQuery()
          }`"
          @dataLoaded="isTableLoading = false"
          @rowDblClick="rowDoubleClick"
          @rowClick="rowSelect"
          :tableKey="tableFlag"
          :key="tableKey"
          :loading="isTableLoading"
          @getPagingInfo="transPagingInfo"
          @showToast="showToast"
          @cloneEntity="cloneEntity"
          @deleteEntity="deleteEntity"
        ></Table>
        
        <Paging
          :pageNumber="pageNumber"
          :pageSize="pageSize"
          :totalPage="totalPage"
          :totalRecord="totalRecord"
          :items="pageItems"
          :entityName="entityName"
          @pageSizeChange="onPageSizeChange"
          @pageNumChange="onPageNumChange"
          @showToast="showToast"
        ></Paging>
      </div>
    </div>
    <Form
      :isOpen="formStatus"
      :close="closeForm"
      :mode="formMode"
      :detailId="entityId"
      :moreDetail="moreDetail"
      @saveClicked="formSaveButtonClick"
      @showToast="showToast"
      @showPopup="showPopup"
      ref="form"
    ></Form>

    <Popup
      :content="popup.content"
      :isHide="popup.isHide"
      :popupType="popup.popupType"
      :buttons="popup.buttons"
      :icon="popup.icon"
      @closePopup="closePopup"
      @popupCallbackFinish="finishPopupCallback"
    ></Popup>
    <div class="toast-stack">
      <Toast
        :type="t.type"
        :header="t.header"
        :msg="t.msg"
        v-for="(t, i) in toasts"
        :key="i"
        transition="fade"
        stagger="1000"
      ></Toast>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import EventBus from "../../event-bus/EventBus";
import BaseButtonIcon from "./BaseButtonIcon.vue";
import Form from "./BaseForm.vue";
import Table from "./BaseTable.vue";
import Popup from "./BasePopup.vue";
import Toast from "./BaseToast.vue";
import Paging from "./BasePaging.vue";

export default {
  name: "Content",
  components: {
    BaseButtonIcon,
    Paging,
    Form,
    Table,
    Popup,
    Toast,
  },
  props: {
    entityName: {
      type: String,
      required: true,
      default() {
        return "Employee";
      },
    },
    filterNames: {
      type: Array,
      required: false,
    },
    headers: {
      type: Array,
      require: true,
    },
    features: {
      type: Array,
      required: false,
    },
  },
  data() {
    return {
      formMode: 0,
      entityId: null,
      entityDetail: null,
      moreDetail: null,
      // Delete info
      deleteIdList: [],
      deleteCodeList: [],
      tableFlag: false,
      tableKey: false,
      // page
      pageSize: 20,
      pageNumber: 0, // = current - 1
      totalRecord: 20,
      totalPage: 1,
      pageItems: [],
      // search
      searchInput: "",
      searchTimeOut: null,
      // Filter
      filter: {
        DepartmentId: "",
        PositionId: "",
        CustomerGroupId: "",
      },
      popup: {
        title: "Empty Title",
        content: "Empty Content",
        popupType: "error",
        okAction: "OK",
        isHide: true,
        callback: null
      },
      formStatus: false,
      isTableLoading: true,
      toasts: [],
      entityMap: {
        Customer: this.$resourceVn.Customer,
        Employee: this.$resourceVn.Employee,
      },
    };
  },
  created() {
    EventBus.$on("showPopup", (args) => {
      this.showPopup(args);
    });
    EventBus.$on("showToast", (type, header, msg, delay) => {
      this.showToast(type, header, msg, delay);
    })
  },
  mounted(){
    this.$nextTick(() => {

      // refresh table khi load trang
      this.refreshTableSelected();
    })
  },
  computed: {
    /**
     * Toggle trạng thái nút xóa nv theo số dòng được select
     * CreatedBy: HungNguyen81 (07-2021)
     */
    delBtnActive() {
      return this.deleteIdList.length > 0;
    },
  },
  watch: {
    // watch sự thay đổi của nội dung tìm kiếm
    searchInput: function (c) {
      if (this.searchTimeOut) clearTimeout(this.searchTimeOut);
      this.searchTimeOut = setTimeout(() => {
        // re-render table
        this.pageNumber = 0;
        this.tableFlag = !this.tableFlag;

        console.log("search:", c);
      }, 500);
    },

    // chuyển trang => render bảng
    pageNumber: function () {
      this.forceTableRerender();
    },
  },
  methods: {
    //#region Xử lí liên quan đến Form

    /**
     * Mở form thông tin nv
     */
    // CreatedBy: HungNguyen81 (07-2021)
    // ModifiedBy: HungNguyen81 (18-08-2021)

    OpenForm(mode) {
      this.formMode = mode;
      this.formStatus = true;
    },

    /**
     * Đóng form
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    closeForm(isChange, mode, id, detail) {
      if (isChange) {
        this.closeFormChanged(mode, id, detail);
      } else {
        this.formStatus = false;
        this.popup.isHide = true;
      }
    },

    /**
     * Đóng form mà k lưu
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (21-08-2021) - Đổi tên method
     */
    closeFormChanged(mode, id, detail) {
      this.popup = {
        content: this.$resourceVn.DataChangeMsg,
        popupType: "info",
        isHide: false,
        buttons: [
          { type: "cancel-button", callback: this.closePopup, value: "Hủy" },
          { type: "no-button", callback: () => {this.closePopup(); this.closeForm(0);}, value: "Không" },
          { type: "yes-button", callback: () => {this.excuteFormModeAction(mode);}, value: "Có" },
        ],
      };
      this.entityId = id;
      this.entityDetail = detail;
    },
    //#endregion

    //#region Xử lý liên quan đến popup thông báo

    /**
     * Đóng popup
     * CreatedBy: HungNguyen81 (07-2021)
     */
    closePopup() {
      this.popup.isHide = true;
      EventBus.$emit("PopupClose");
    },
    /**
     * Hiển thị popup khi có callback
     * CreatedBy: HungNguyen81 (07-2021)
     */
    showPopup(options) {
      for (let btn of options.buttons) {
        if (!btn.callback) {
          btn.callback = this.closePopup;
        }
      }
      this.popup = options;
    },

    /**
     * Ẩn popup sau khi callback của nút OK chạy xong
     * CreatedBy: HungNguyen81 (07-2021)
     */
    finishPopupCallback() {
      this.popup.isHide = true;
    },
    //#endregion

    //#region Xử lý liên quan đến toast message
    /**
     * Hiển thị toast message
     * @param {String} type
     * @param {String} header
     * @param {String} msg
     * @param {Number} delay Thời gian trễ khi hiển thị toast, mặc định là 0
     */
    showToast(type, header, msg, delay) {
      const Show = () => {
        this.toasts.push({
          type: type,
          header: header.toUpperCase(),
          msg: msg,
        });
      };
      if (delay) {
        setTimeout(Show, delay);
      } else {
        Show();
      }
    },
    //#endregion

    //#region Handle sự kiện emit

    /**
     * Handle khi chuyển trang trên paging component
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    onPageSizeChange(size) {
      this.pageSize = size;
      this.pageNumber = 0;
      this.refreshTableSelected();
    },

    /**
     * Handle khi chọn trang ở paging bar
     * CreatedBy: HungNguyen81 (08-2021)
     */
    onPageNumChange(num) {
      this.pageNumber = num;
    },

    /**
     * khi dữ liệu table load xong, gửi totalPage, totalRecord sang paging component
     * CreatedBy: HungNguyen81 (07-2021)
     */
    transPagingInfo(numPage, numRecord) {
      this.totalRecord = numRecord;
      this.totalPage = numPage;
    },


    handleScroll(event){
      EventBus.$emit('scrollView', event.target.scrollTop, event.target.scrollLeft)
    },

    //#endregion

    //#region xử lí sự kiện DOM
    /**
     * Hiện popup thông báo xác nhận thêm/sửa
     * CreatedBy: HungNguyen81 (07-2021)
     */
    formSaveButtonClick(mode, id, detail, callback) {
      this.entityId = id;
      this.entityDetail = detail;
      this.excuteFormModeAction(mode, callback);
    },

    /**
     * Thực thi tác vụ tương ứng với form mode
     * CreatedBy: HungNguyen81 (29-08-2021)
     */
    excuteFormModeAction(mode, callback){
      if(mode == this.$config.FORM_ADD){
        this.sendPostRequest(callback);
      } else if(mode == this.$config.FORM_UPDATE){
        this.sendPutRequest(callback);
      }
    },

    /**
     * Gọi hàm khi bấm nút Thêm nhân viên/ Thêm Khách hàng
     * CreatedBy: HungNguyen81 (07-2021)
     */
    btnAddClick() {
      this.OpenForm(this.$config.FORM_ADD);
    },

    // Xử lí sự kiện click đúp vào table row
    // CreatedBy: HungNguyen81 (07-2021)
    rowDoubleClick(id, pos, dep) {
      this.entityId = id;
      this.moreDetail = {
        PositionName: pos,
        DepartmentName: dep,
      };
      console.log("id", id);
      this.OpenForm(this.$config.FORM_UPDATE);
    },

    /**
     *  Xử lí khi select 1 table row
     * CreatedBy: HungNguyen81 (07-2021)
     */
    rowSelect(id, code) {
      if (this.deleteIdList.includes(id)) {
        let i = this.deleteIdList.indexOf(id);
        this.deleteIdList.splice(i, 1);
        this.deleteCodeList.splice(i, 1);
      } else {
        this.deleteIdList.push(id);
        this.deleteCodeList.push(code);
      }
    },

    /**
     * Xử lí sự kiện bấm nút xóa các hàng đã chọn trên table
     * CreatedBy: HungNguyen81 (07-2021)
     */
    delBtnClick() {
      this.showPopup({
        content: this.$resourceVn.EmployeeDeleteMsg.replace('@', this.deleteCodeList.join(", ")),
        popupType: "warning",
        isHide: false,
        buttons: [
          { type: "cancel-button", callback: null, value: this.$resourceVn.NoButtonText },
          {
            type: "yes-button",
            callback: this.sendDeleteRequests,
            value: this.$resourceVn.YesButtonText,
          },
        ],
      });
    },

    cloneEntity(id){
      this.entityId = id;
      this.OpenForm(this.$config.FORM_CLONE);
    },

    deleteEntity(id, code){
      this.showPopup({
        content: this.$resourceVn.EmployeeDeleteMsg.replace('@', code),
        popupType: "warning",
        isHide: false,
        buttons: [
          { type: "cancel-button", callback: null, value: this.$resourceVn.NoButtonText },
          {
            type: "yes-button",
            callback: ()=>{this.sendDeleteOneRequest(id)},
            value: this.$resourceVn.YesButtonText,
          },
        ],
      });
    },
    //#endregion

    //#region Requests

    /**
     * Xóa một entity
     * CreatedBy: HungNguyen81 (29-08-2021)
     */
    sendDeleteOneRequest(id){
      axios
        .delete(`${this.$config.BASE_API}/${this.entityName}s/${id}`)
        .then((res) => {
          // Đóng popup và làm mới bảng
          this.closePopup();
          this.forceTableRerender();

          // hiển thị toast thông báo đã xóa thành công
          this.showToast("info", this.$resourceVn.DeleteSuccessTitle, res.data.Msg);
        })
        .catch(() => {
          this.showToast("error", "Delete error", this.$resourceVn.DeleteFailTitle);
        });
    },
    /**
     * Callback khi bấm OK trong popup, gửi toàn bộ id cần xóa lên 1 request
     * CreatedBy: HungNguyen81 (07-2021)
     */
    sendDeleteRequests() {
      this.forceTableRerender();
      axios
        .delete(`${this.$config.BASE_API}/${this.entityName}s/`, {
          data: this.deleteIdList,
        })
        .then((res) => {
          this.deleteIdList = [];
          this.deleteCodeList = [];

          // Đóng popup và làm mới bảng
          this.closePopup();
          this.forceTableRerender();

          // hiển thị toast thông báo đã xóa thành công
          this.showToast("info", this.$resourceVn.DeleteSuccessTitle, res.data.Msg);
        })
        .catch(() => {
          this.showToast("error", "Delete error", this.$resourceVn.DeleteFailTitle);
        });
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode sửa nhân viên
     * CreatedBy: HungNguyen81 (07-2021)
     */
    sendPutRequest(callback) {
      axios
        .put(
          `${this.$config.BASE_API}/${this.entityName}s/${this.entityId}`,
          this.entityDetail
        )
        .then(() => {
          // this.closePopup();
          // this.closeForm();
          if(callback) callback();
          this.showToast(
            "success",
            "PUT Success",
            this.$resourceVn.UpdateSuccessMsg.replace('@', this.entityDetail.FullName)
          );

          this.forceTableRerender();
        })
        .catch((err) => {
            this.handleInvalidResponse(err);
        });
    },

    /**
     * Callback khi bấm nút Lưu trên form, form mode thêm nhân viên
     * CreatedBy: HungNguyen81 (07-2021)
     */
    sendPostRequest(callback) {
      axios
        .post(
          `${this.$config.BASE_API}/${this.entityName}s/`,
          this.entityDetail
        )
        .then((res) => {
          console.log(111);
          if(callback) {
            console.log("111");
            callback();
          }
          this.showToast("success", this.$resourceVn.PostSuccessTitle, res.data.Msg);
          this.forceTableRerender();
        })
        .catch((err) => {
          this.handleInvalidResponse(err);
        });
    },

    handleInvalidResponse(err){
      var func = () => {
        this.closePopup();
        this.$refs.form.$refs['employeeCode'].$el.children[1].focus();
      }
      this.showToast("error", this.$resourceVn.PutErrorTitle, err.response.data.Msg);
      this.showPopup({
        content: err.response.data.Msg,
        popupType: "warning",
        isHide: false,
        buttons: [
          { type: "yes-button", callback: func, value: this.$resourceVn.ConfirmButtonText },
        ],
      })
      EventBus.$emit('requestFail');
    },

    //#endregion

    //#region Làm mới bảng dữ liệu
    /**
     * Refresh table và bỏ các dòng đã selected
     * CreatedBy: HungNguyen81 (07-2021)
     */
    refreshTableSelected() {
      localStorage.setItem("select", JSON.stringify([]));
      this.deleteIdList = [];
      this.deleteCodeList = [];
      this.forceTableRerender();
    },

    /**
     * Khiến table phải re-render
     * CreatedBy: HungNguyen81 (07-2021)
     */
    forceTableRerender() {
      this.isTableLoading = true;
      this.tableFlag = !this.tableFlag;
    },

    //#endregion

    /**
     * Lấy thông số filter cho api query
     * CreatedBy: HungNguyen81 (07-2021)
     */
    getApiFilterQuery() {
      var res = "";
      for (let i = 0; i < this.filterNames.length; i++) {
        res += `&${this.filterNames[i]}=${this.filter[this.filterNames[i]]}`;
      }
      return res;
    },
  },
};
</script>

<style scoped>
@import "../../css/layout/content.css";

.toast-stack {
  position: fixed;
  bottom: 0;
  right: 0;
  height: calc(100vh - 49px);
  width: 8px;
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 100;
}
</style>
