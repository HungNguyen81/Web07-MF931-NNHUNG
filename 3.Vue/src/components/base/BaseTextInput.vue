<template>
  <div class="input-field tooltip" :id="inputKey">
    <span class="tooltip-text" v-if="!isValidate">{{ invalidTooltip }}</span>
    <div class="input-label">
      {{ label }} <span v-if="required">(<span class="required">*</span>)</span>
    </div>

    <input
      spellcheck="false"
      maxlength="255"
      @blur="inputValidate()"
      :type="valueType"
      :class="['textbox-default', type, { invalid: !isValidate }]"
      v-bind:value="value"
      v-on="inputListeners"
      :tabindex="tabindex"
      @keydown="onInput"
    />
  </div>
</template>

<script>
export default {
  name: "BaseTextInput",
  components: {},
  props: {
    inputKey: {
      type: String,
      required: false,
    },
    value: {
      type: String,
    },
    valueType: {
      type: String,
    },
    // bổ sung class cho nhiều loại css khác nhau
    type: {
      type: String,
    },
    // mảng các hàm validate áp dụng lên input
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
    pattern:{
      type: RegExp,
      require: false
    },
    
  },
  data() {
    return {
      isValidate: true,
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
  },
  watch: {
    /**
     * Tắt border cảnh báo invalid khi mở form (hoặc re-render form)
     * CreatedBy: HungNguyen81 (08-2021)
     */
    rerenderFlag: function () {
      this.isValidate = true;
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
        let msg = "";
        for (let func of this.validates) {
          let valid = func(this.label, this.value);
          res = res && valid.isValid;
          msg = this.invalidTooltip = valid.msg;
          if (!res) break;
        }
        this.isValidate = res;

        this.$emit("valid", this.inputKey, res);
        return {
          IsValid : res,
          Msg: msg
        };
      } else {
        console.log("NO validations");
        return {
          IsValid: true
        };
      }
    },
    onInput(e){
      if(!this.pattern) return;
      if(!/^(Tab)|^(Backspace)|^(Shift)|^(Home)|^(End)|^(Arrow)/.test(e.key)
      && !this.pattern.test(e.key)){
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