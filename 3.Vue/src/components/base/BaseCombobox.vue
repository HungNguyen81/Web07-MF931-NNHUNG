<template>
  <div class="combobox-container" v-if="isDataLoaded">
    <div class="combobox tooltip combobox-tooltip" ref="combobox">
      <input
        spellcheck="false"
        type="text"
        :class="['combobox-input', 'textbox-default', { invalid: !isValidate }]"
        :tabindex="tabindex"
        
        @focus="handleComboboxInput()"
        @blur="inputValidate()"
        @keyup="handleKeyPress($event)"
        v-bind:value="value"
        v-on="inputListeners"
        ref="comboboxInput"
      />
      <span class="tooltip-text" v-if="!isValidate">{{ invalidTooltip }}</span>
      <div class="combobox-icon-container" @click="toggleDropList()">
        <div class="combobox-icon"></div>
      </div>
    </div>
    <div
      :class="['dropdown-data', dropListClass, { hide: isHide }]"
      ref="dropList"
    >
      <div
        :class="[
          'dropdown-item',
          { 'item-selected': index == current, hide: item.Hidden },
        ]"
        v-for="(item, index) in items"
        :key="index"
        @click="itemSelect(item, index)"
      >
        <div class="item-text">{{ item[typeDataKey] }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import EventBus from "../../event-bus/EventBus";
import axios from "axios";
import utils from "../../mixins/ultis";
export default {
  name: "Combobox",
  components: {},
  // type: Position, Department,
  props: {
    type: {
      type: String,
      required: false,
    },
    typeDataKey: {
      type: String,
    },
    api: {
      type: String,
      required: false,
    },
    mode: {
      type: Number,
      require: false,
      default() {
        return 0;
      },
    },
    data: {
      type: Array,
      required: false,
    },
    // initValue: {
    //   type: String,
    //   require: false,
    // },
    tabindex: {
      type: Number,
      required: false,
    },
    // mảng các hàm validate áp dụng lên input
    validates: {
      type: Array,
    },
    label: {
      type: String,
    },
    value: {
      type: String,
    },
    rerenderFlag: {
      type: Boolean,
      require: false
    }
  },
  mixins: [utils],
  data() {
    return {
      current: -1,
      // Ẩn hiện drop list
      isHide: true,
      isDataLoaded: false,
      typeName: this.typeDataKey,
      items: [],
      map: {
        Position: "vị trí",
        Department: "phòng ban",
        CustomerGroup: "nhóm khách hàng",
        PageSize: "Kích thước trang",
        Unit: "Đơn vị",
      },
      isEmptyVal: true,
      isValidate: true,
      invalidTooltip: "",
    };
  },
  created() {
    // nếu truyền vào API url
    if (this.api) {
      axios
        .get(this.api)
        .then((res) => {
          this.items = [];

          if (this.mode == 1 && this.type) {
            this.items.push({
              [this.typeName]: "Tất cả " + this.map[this.type],
              [this.type + "Id"]: "",
            });
          }
          // res.data.forEach((e) => {
          //   this.items.push(e);
          // });
          Object.assign(this.items, res.data);

          this.items = this.items.map((e) => ({
            ...e,
            Hidden: false,
          }));

          this.isDataLoaded = true;
          if (this.value) {
            if (this.data) {
              this.data.forEach((e, i) => {
                if (e[this.typeDataKey] == this.value) {
                  this.current = i;
                }
              });
            }
          }
        })
        .catch((err) => {
          console.log(err);
          this.$emit(
            "showToast",
            "error",
            "SERVER ERROR",
            `Cannot load ${this.type}!`
          );
          this.isDataLoaded = true;
        });
    } else {
      // nếu truyền vào data cố định
      this.items = this.data.map((e) => ({
        ...e,
        Hidden: false,
      }));

      this.isDataLoaded = true;
      // if (this.initValue) {
      //   this.value = this.initValue;
        this.data.forEach((e, i) => {
          if (e[this.typeDataKey] == this.value) {
            this.current = i;
          }
        });
      // }
    }
  },
  mounted() {
    EventBus.$on("appClick", (target) => {
      var container = this.$refs.combobox;
      if (!container) return;
      if (!container.contains(target)) {
        this.isHide = true;
      }
    });
  },
  computed: {
    inputListeners: function () {
      var vm = this;
      return Object.assign(
        {},
        // We add all the listeners from the parent
        this.$listeners,
        // Then we can add custom listeners or override the
        // behavior of some listeners.
        {
          // This ensures that the component works with v-model
          input: function (event) {
            vm.$emit("input", event.target.value);
          },
        }
      );
    },
    /**
     * Compute class cho combobox
     */
    dropListClass: function () {
      if (!this.type) return "";
      return this.type.toLowerCase() + "s";
    },
  },
  watch: {
    // giá trị hiển thị trên input
    value: function (value) {
      this.isEmptyVal = !(value || (value && value.trim()));
      if (this.isEmptyVal) {
        this.current = -1;
        this.items = this.items.map((e) => ({
          ...e,
          Hidden: false,
        }));
      }
      // this.data.forEach((e, i) => {
      //     if (e[this.typeDataKey] == value) {
      //       this.current = i;
      //     }
      // });
    },
    
    // chỉ số của item hiện tại
    current: function (c) {
      if (c < 0) return;
      // this.value = this.items[c][this.typeDataKey];

      if (c >= 3) {
        this.$refs.dropList.scrollTop = 40 * (c - 3);
      } else if (c == 0) {
        this.$refs.dropList.scrollTop = 0;
      }
    },

    /**
     * Tắt border cảnh báo invalid khi mở form (hoặc re-render form)
     */
    rerenderFlag: function () {
      this.isValidate = true;
    },
  },
  methods: {
    /**
     * Kiểm tra tính hợp lệ bằng cách gọi lần lượt các hàm validate truyền vào từ props
     * CreatedBy: HungNguyen81 (30-08-2021)
     */
    inputValidate() {
      console.log("VALIDATE");
      if (this.validates) {
        let res = true;
        let msg = "";
        for (let func of this.validates) {
          let valid = func(this.label, this.value);
          res = res && valid.isValid;
          msg = this.invalidTooltip = valid.msg;
          if (!res) break;
        }
        this.isValidate = res;

        this.$emit("valid", this.typeDataKey, res);
        return {
          IsValid: res,
          Msg: msg,
        };
      } else {
        console.log("NO validations");
        return {
          IsValid: true,
        };
      }
    },

    /**
     * Handle khi click chọn item
     * CreatedBy: HungNguyen81 (07-2021)
     */
    itemSelect(item, index) {
      this.current = index;
      this.isHide = true;
      this.$emit("itemChange", this.typeDataKey, item);
      this.isValidate = true;
      this.$refs.comboboxInput.focus();
    },

    /**
     * handle khi bấm phím
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (20-08-2021) <Sửa lại code xử lý khi bấm phím mũi tên>
     */
    handleKeyPress(event) {
      console.log("keyup");

      let maxOffset = this.items.length; //0;
      // for (let item of this.items) {
      //   maxOffset = maxOffset + (item.Hidden ? 0 : 1);
      // }

      if (event.code == "ArrowDown") {
        event.preventDefault();

        do {
          this.current = this.current < 0 ? -1 : this.current;
          this.current = (this.current + 1) % maxOffset;
        } while (this.items[this.current].Hidden == true);
      } else if (event.code == "ArrowUp") {
        event.preventDefault();

        do {
          this.current = this.current < 0 ? 0 : this.current;
          this.current = this.current == 0 ? maxOffset - 1 : this.current - 1;
        } while (this.items[this.current].Hidden == true);
      } else if (event.code == "Enter") {
        this.isHide = true;
        this.$emit("itemChange", this.typeDataKey, this.items[this.current]);
      } else {
        this.handleComboboxInput()
      }
    },

    /**
     * Handle khi client nhập vào combobox
     * CreatedBy: HungNguyen81 (07-2021)
     */
    handleComboboxInput() {
      console.log("input", this.value);
      this.isHide = false;

      if (!this.value) {
        return;
      }

      if (this.items) {
        for (let item of this.items) {
          let comparedString = this.value
            ? this.removeAccents(this.value.toUpperCase())
            : " ";
          let itemString = this.removeAccents(
            item[this.typeDataKey].toUpperCase()
          );
          if (itemString.includes(comparedString)) {
            item.Hidden = false;
          } else {
            item.Hidden = true;
          }
        }
      }
    },

    emptyInput() {
      // this.value = "";
      this.items = this.items.map((e) => ({
        ...e,
        Hidden: false,
      }));
      this.current = -1;
      this.$refs.comboboxInput.focus();
      this.$emit("filterActive", this.type, "");
    },

    showAllItems() {
      this.items = this.items.map((e) => ({
        ...e,
        Hidden: false,
      }));
    },

    toggleDropList() {
      this.isHide = !this.isHide;
      if (!this.isHide) {
        this.showAllItems();
      }
      // this.$nextTick(() => {
      //   this.$refs.comboboxInput.focus();
      // });
    },
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
@import "../../css/base/combobox.css";
@import "../../css/base/tooltip.css";
</style>