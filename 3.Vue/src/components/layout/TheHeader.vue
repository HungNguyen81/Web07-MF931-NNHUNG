<template>
  <div class="header">
    <div class="header-left">
      <div class="toggle"></div>
      <div class="logo">
        <img
          src="https://actappg1.misacdn.net/img/Logo_Module_TiengViet_White.66947422.svg"
          alt="logo"
        />
      </div>
    </div>
    <div class="header-right">
      <div class="collapse-btn" @click="collapse"></div>
      <div class="company-label">
        {{ $resourceVn.CompanyName }}
      </div>
      <div class="mode-btn"></div>
      <div class="user-options">
        <div class="notification"></div>
        <div class="user-avatar"></div>
        <div class="username">Nguyễn Ngọc Hưng</div>
        <div class="icon-options"></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Header",
  components: {},
  props:{
    collapse:{
      type: Function,
      require: false
    }
  },
  data() {
    return {
      isHide: true,
    };
  },
};
</script>

<style scoped>
@import "../../css/layout/header.css";
@import "../../css/base/dropdown.css";
</style>
