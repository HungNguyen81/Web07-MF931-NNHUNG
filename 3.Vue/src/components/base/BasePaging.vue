<template>
  <div class="content-page-navigator">
    <div class="navigator-left" id="current-pagesize">
      {{ $resourceVn.LabelTotalRecord.replace('@', totalRecord) }}
    </div>
    <div class="navigator-center"></div>
    <div class="navigator-right">
      <Combobox
        :mode="1"
        :data="pageSizeDropData"
        :type="'PageSize'"
        class="drop-number-of-row"
        :typeDataKey="'PageSizeName'"
        v-model="pageSizeComboboxValue"
        @itemChange="pageSizeChange"
      ></Combobox>
      <div
        class="button-prev-page button-navigator"
        :class="{ disable: current == 1 }"
        @click="prev"
      >
        {{ $resourceVn.NextButtonText }}
      </div>
      <div class="page-buttons">
        <div
          class="button-page-number"
          @click="$emit('pageNumChange', 0)"
          v-if="isShowFirst && current > 3"
        >
          1
        </div>
        <div
          class="button-page-number"
          v-if="isShowFirst && current > 3"
          @click="toggleShow"
        >
          ...
        </div>
        <div
          class="button-page-number"
          :class="{ 'button-current-page': current == item + offset - 1 }"
          v-for="(item, index) in Math.min(3, allPage)"
          :key="index"
          @click="$emit('pageNumChange', item + offset - 2)"
        >
          {{ item + offset - 1 }}
        </div>
        <div
          class="button-page-number"
          v-if="isShowLast && allPage - current >= 3"
          @click="toggleShow"
        >
          ...
        </div>
        <div
          class="button-page-number"
          :class="{ 'button-current-page': current == totalPage }"
          @click="$emit('pageNumChange', totalPage - 1)"
          v-if="isShowLast && allPage - current >= 3"
        >
          {{ totalPage }}
        </div>
      </div>
      <div
        class="button-next-page button-navigator"
        :class="{ disable: current == allPage }"
        @click="next"
      >
        {{ $resourceVn.PrevButtonText }}
      </div>
    </div>
  </div>
</template>

<script>
import Combobox from "./BaseCombobox.vue";

export default {
  name: "PagingBar",
  components: {
    Combobox,
  },
  props: {
    pageNumber: {
      type: Number,
      require: true,
    },
    pageSize: {
      type: Number,
      require: true,
    },
    totalRecord: {
      type: Number,
      require: true,
    },
    totalPage: {
      type: Number,
      require: true,
    },
    items: {
      type: Array,
    },
    entityName: {
      type: String,
      require: true,
      default() {
        return "Employee";
      },
    },
  },
  data() {
    return {
      entityNameMap: {
        Employee: this.$resourceVn.Employee,
        Customer: this.$resourceVn.Customer,
      },
      pageSizeDropData: this.$resourceVn.PageSizeDropData,
      page: {
        start: 1,
        end: 20,
      },
      pSize: 20,
      current: 1,
      allPage: 1,
      isShowFirst: false,
      isShowLast: true,
      pageSizeComboboxValue: "",
    };
  },
  mounted() {
    this.pSize = this.pageSize;
    this.current = this.pageNumber + 1;
    this.allPage = this.totalPage;
  },
  created() {
    this.pageSizeComboboxValue = this.pageSizeDropData[1].PageSizeName;
  },
  watch: {
    totalPage: function (tp) {
      this.allPage = tp;
      if (tp < 6) {
        this.isShowFirst = this.isShowLast = false;
      }
    },
    pageNumber: function (c) {
      this.current = c + 1;
      if (this.current == this.totalPage) {
        this.isShowFirst = true;
        this.isShowLast = false;
      } else if (this.current < 4) {
        this.isShowFirst = false;
        this.isShowLast = true;
      }
    },
  },
  computed: {
    startRow: function () {
      return Math.min(
        (this.current - 1) * this.pSize + 1,
        Number(this.totalRecord)
      );
    },

    endRow: function () {
      return Math.min(this.current * this.pSize, Number(this.totalRecord));
    },

    offset: function () {
      return this.current < 2
        ? 1
        : this.current - 1 - Math.max(0, 1 - this.allPage + this.current);
    },
  },
  methods: {
    toggleShow() {
      if (this.allPage - this.current < 3) return;
      if (this.current < 4) return;
      this.isShowFirst = !this.isShowFirst;
      this.isShowLast = !this.isShowLast;
    },

    /**
     * Handle khi có thay đổi trong dropdown chọn kích thước trang
     */
    // CreatedBy: HungNguyen81 (18-08-2021)
    // ModifiedBy: HungNguyen81 (18-08-2021)
    pageSizeChange(type, data) {
      this.pSize = data.Size;
      this.pageSizeComboboxValue = data.PageSizeName;
      this.$emit("pageSizeChange", this.pSize);
      document.getElementById("table-view").style.height = `${
        this.pSize * 48 + 34 + 56
      }px`;
    },

    /**
     * Chuyển sang trang kế tiếp
     * CreatedBy: HungNguyen81 (18-08-2021)
     */
    next() {
      this.current = this.current < this.totalPage ? this.current + 1 : 1;
      console.log("next", this.current + "/" + this.totalPage);
      this.$emit("pageNumChange", this.current - 1);
    },

    /**
     * Chuyển về trang ngay trước
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    prev() {
      this.current = this.current > 1 ? this.current - 1 : this.totalPage;
      console.log("prev", this.current + "/" + this.totalPage);
      this.$emit("pageNumChange", this.current - 1);
    },

    /**
     * Chuyển về trang đầu tiên
     * CreatedBy: HungNguyen81 (18-08-2021)
     * ModifiedBy: HungNguyen81 (18-08-2021)
     */
    first() {
      this.current = 1;
      this.$emit("pageNumChange", 0);
    },

    /**
     * Chuyển tới trang cuối cùng
     * CreatedBy: HungNguyen81 (18-08-2021)
     */
    last() {
      this.current = this.totalPage;
      this.$emit("pageNumChange", this.current - 1);
    },
  },
};
</script>
<style scoped>
@import "../../css/base/paging-bar.css";
</style>