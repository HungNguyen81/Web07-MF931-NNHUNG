<template>
  <div :class="['popup-wrapper', { open: !isHide, close: isHide }]">
    <div :class="['popup-container', { open: !isHide, close: isHide }]">
      <div class="popup-body">
        <div :class="['msg-icon', { [popupType]: [popupType] }]"></div>
        <div class="msg-content">
          <p><span>{{content}}</span></p>
        </div>
      </div>
      <div class="popup-footer">
        <base-button
          class="popup-button"
          :type="btn.type"
          :onclick="() => {btn.callback(); $emit('popupCallbackFinish');}"
          :value="btn.value"
          v-for="(btn, i) in buttons" :key="i"
        ></base-button>
      </div>
    </div>
  </div>
</template>

<script>
import BaseButton from "./BaseButtonIcon.vue";
export default {
  name: "Popup",
  components: {
    BaseButton,
  },
  props: {
    content: {
      type: String,
      require: false,
    },
    // error, warning, info
    popupType: {
      type: String,
      require: true,
    },
    icon:{
      type: String,
      required: false
    },
    isHide: {
      type: Boolean,
      require: true,
    },
    buttons:{
      type: Array,
      required: false,
      default(){
        return [{type:"button-ok", callback: () => {this.$emit('closePopup');}, value:"Đóng"}];
      }
    }
  },
  mounted() {},
  methods: {
    // /**
    //  * Handle khi bấm nút OK, gọi hàm callback nếu có
    //  * @ CreatedBy: HungNguyen81 (08-2021)
    //  */
    // handleOkClick() {
    //   if (this.callback) {
    //     this.callback(); this.$emit("popupCallbackFinish");
    //   } else {
    //     console.log("Callback function not found!");
    //   }
    // },
  },
  watch: {
    // isHide: function (val) {
    //   if (!val)
    //   this.$nextTick(() => {
    //     this.$refs.CancelBtn.$el.focus();
    //   });
    // },
  },
};
</script>

<style scoped>
@import '../../css/components/popup.css';
</style>