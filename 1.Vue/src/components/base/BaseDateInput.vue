<template>
  <div class="input-field tooltip" :id="inputKey">
    <span class="tooltip-text" v-if="!isValidate">{{ invalidTooltip }}</span>
    <div class="input-label">
      {{ label }} <span v-if="required">(<span class="required">*</span>)</span>
    </div>

    <div
      :class="[
        'date-input',
        'textbox-default',
        type,
        { 'input-focus': isDateFocus },
        { invalid: !isValidate },
      ]"
    >
      <input
        type="text"
        placeholder="DD/MM/YYYY"
        class="date-edit"
        :tabindex="tabindex"
        v-model="formatedValue"
        @focus="isDateFocus = true;"
        @blur="isDateFocus = false;inputValidate();"
        @keydown="onInput"
        ref="dateView"
      />
      <input
        @blur="isDateFocus = false; inputValidate();"
        @focus="isDateFocus = true"
        type="date"
        v-bind:value="value"
        v-on="inputListeners"
        :tabindex="tabindex"
        :max="maxDateValue"
      />
    </div>
  </div>
</template>

<script>
import ultis from '../../mixins/ultis.js'
export default {
  name: "BaseDateInput",
  components: {},
  mixins:[ultis],
  props: {
    inputKey: {
      type: String,
      required: false,
    },
    value: {
      type: String,
    },
    type: {
      type: String,
    },
    validates: {
      type: Array,
    },
    label: {
      type: String,
    },
    required: {
      type: Boolean,
    },
    tabindex: {
      type: Number,
    },
    rerenderFlag: {
      type: Boolean,
      require: false,
    },
    maxDate:{
      type: String,
      required: false
    },
  },
  data() {
    return {
      isValidate: true,
      isDateFocus: false,
      formatedValue: null,
      dateTimeOut: null,
      invalidTooltip: "",
    };
  },
  mounted() {},
  computed: {
    inputListeners: function () {
      var vm = this;
      return Object.assign(
        {},
        this.$listeners,
        {
          input: function (event) {
            vm.$emit("input", event.target.value);
          },
        }
      );
    },
    maxDateValue: function(){
      if(this.maxDate) return this.maxDate;
      return this.dateFormatVer2(new Date(), 'yyyy-MM-dd');
    }
  },
  watch: {
    /**
     * Tắt border cảnh báo invalid khi mở form (hoặc re-render form)
     * CreatedBy: HungNguyen81 (08-2021)
     */
    rerenderFlag: function () {
      this.isValidate = true;
    },

    /**
     * Watch sự thay đổi của input value
     * CreatedBy: HungNguyen81 (08-2021)
     */
    value: function (val) {
      if (!val) {
        this.formatedValue = null;
        return;
      }
      
      this.formatedValue = this.dateFormatVer2(val, "dd/MM/yyyy");
    },
    
    /**
     * Watch sự thay đổi của giá trị ngày đc hiển thị lên input
     * CreatedBy: HungNguyen81 (08-2021)
     */
    formatedValue: function (val) {
      clearTimeout(this.dateTimeOut);
      let newVal;
      if (!val) {
        newVal = "";
      } else {
        let data = val.split("/");
        if (
          data.length < 3 ||
          !data[2] ||
          !data[1] ||
          !data[0] ||
          data[2].length < 4
        )
          return;
        let yyyy = this.zeroPad(data[2], 4);
        let mm = this.zeroPad(data[1], 2);
        let dd = this.zeroPad(data[0], 2);

        newVal = `${yyyy}-${mm}-${dd}`;
        // if(isNaN(Date.parse(new Date(newVal)))){
        //   newVal = ""
        // }
      }

      this.dateTimeOut = setTimeout(() => {
        this.$emit(
          "dateChange",
          this.inputKey,
          newVal,
          this.$refs.dateView,
          val
        );
      }, 300);
    },
  },
  methods: {
    /**
     * Kiểm tra tính hợp lệ bằng cách gọi lần lượt các hàm validate truyền vào từ props
     * CreatedBy: HungNguyen81 (08-2021)
     */
    inputValidate() {
      if (this.validates) {
        let res = true;
        for (let func of this.validates) {
          let valid = func(this.label, this.value);
          res = res && valid.isValid;
          this.invalidTooltip = valid.msg;
          if (!res) break;
        }
        this.isValidate = res;

        this.$emit("valid", this.inputKey, res);
        return {
          IsValid: res,
          Msg: this.invalidTooltip
        }
      } else {
        console.log("NO validations");
        return {
          IsValid: true
        }
      }
    },

    /**
     * Không cho phép nhập kí tự ngoài kí tự số và dấu "/"
     * CreatedBy: HungNguyen81 (23-08-2021)
     */
    onInput(e){
      if(!/([0-9/])|^(Tab)|^(Backspace)|^(Shift)|^(Home)|^(End)|^(Arrow)/.test(e.key)){
        e.preventDefault();
      }
    }
  },
};
</script>

<style scoped>
@import "../../css/base/text-box.css";
@import "../../css/base/tooltip.css";
</style>