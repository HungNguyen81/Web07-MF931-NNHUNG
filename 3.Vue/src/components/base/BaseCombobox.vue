<template>
  <div
    class="combobox-container"
    v-if="isDataLoaded"
  >
    <div class="combobox" ref="combobox">
      <input
        spellcheck="false"
        type="text"
        :class="['combobox-input', 'textbox-default']"
        :tabindex="tabindex"
        ref="comboboxInput"
        @input="comboboxInput"
        @focus="comboboxInput"
        @keyup="handleKeyPress"
        v-model="value"
      />
      <!-- <div :class="['x-icon', { hide: isEmptyVal }]" @click="emptyInput">
        <i class="fas fa-times"></i>
      </div> -->
      <div class="combobox-icon-container" @click="toggleDropList">
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
        <!-- <i class="fas fa-check item-icon"></i> -->
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
  // mode: 1- add "Tat ca ...", 0/null- normal
  // props: ["type", "api", "mode"],
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
    init:{
      type: Number,
      require: false,
    },
    tabindex: {
      type: Number,
      required: false,
    },
  },
  mixins: [utils],
  data() {
    return {
      value: "",
      current: 0,
      // Ẩn hiện drop list
      isHide: true,
      isDataLoaded: false,
      typeName: this.typeDataKey,
      items: [],
      map: {
        Position: "vị trí",
        Department: "phòng ban",
        CustomerGroup: "nhóm khách hàng",
        PageSize: "Kích thước trang"
      },
      isEmptyVal: true,
    };
  },
  created() {
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
          if(this.init){
            this.value = this.items[this.init][this.typeDataKey];
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
      this.items = this.data.map((e) => ({
        ...e,
        Hidden: false,
      }));

      this.isDataLoaded = true;
      this.value = this.items[this.init]? this.items[this.init][this.typeDataKey] : "";
      if(this.value){
        this.current = this.init;
      }
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
    value: function () {
      this.isEmptyVal = !(this.value || this.value.trim());
      if (this.isEmptyVal) {
        this.current = -1;
        this.items = this.items.map((e) => ({
            ...e,
            Hidden: false,
          }));
      }
    },
    // chỉ số của item hiện tại
    current: function (c) {
      if (c < 0) return;
      this.value = this.items[c][this.typeDataKey];

      if (c >= 3) {
        this.$refs.dropList.scrollTop = 40 * (c - 3);
      } else if (c == 0) {
        this.$refs.dropList.scrollTop = 0;
      }
    },
  },
  methods: {
    /**
     * handle khi click chọn 1 item trong list dropdown
     * CreatedBy: HungNguyen81 (07-2021)
     */
    // itemClicked() {
    //   this.isHide = true;
    //   this.$emit(
    //     "filterActive",
    //     this.type,
    //     this.items[this.current][this.type + "Id"]
    //   );
    // },
    /**
     * Handle khi click chọn item
     * CreatedBy: HungNguyen81 (07-2021)
     */
    itemSelect(item, index) {
      this.current = index;
      this.isHide = true;
      this.$emit("itemChange", this.typeDataKey, item);
      this.$refs.combobox.focus();
    },

    /**
     * handle khi bấm phím
     * CreatedBy: HungNguyen81 (07-2021)
     * ModifiedBy: HungNguyen81 (20-08-2021) <Sửa lại code xử lý khi bấm phím mũi tên>
     */
    handleKeyPress(event) {
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
        this.value = this.items[this.current][this.typeDataKey];
        // this.$emit(
        //   "filterActive",
        //   this.type,
        //   this.items[this.current][this.type + "Id"]
        // );
        this.$emit("itemChange", this.typeDataKey, this.items[this.current]);
      }
    },

    /**
     * Handle khi client nhập vào combobox
     * CreatedBy: HungNguyen81 (07-2021)
     */
    comboboxInput() {
      this.isHide = false;
      // if (!this.value) {
      //   this.isHide = false;
      // }
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
      this.value = "";
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
    },
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
@import "../../css/base/combobox.css";
</style>