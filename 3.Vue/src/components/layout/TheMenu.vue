<template>
  <div class="menu">
    <a
      :href="item.path"
      class="menu-item"
      :class="[item.icon, { 'menu-item-active': route == item.path }]"
      v-for="(item, i) in items"
      :key="i"
    >
      <div class="menu-icon"></div>
      <div class="menu-item-text">{{ item.text }}</div>
    </a>
  </div>
</template>

<script>
export default {
  name: "Menu",
  components: {},
  props: {
    items: {
      type: Array,
      required: false,
    },
    route:{
        type: String,
        require: false
    }
  },
};
</script>

<style scoped>
@import "../../css/layout/menu.css";
/* Handle */
::-webkit-scrollbar-thumb {
    background: #D4D7DC;
}
::-webkit-scrollbar-thumb:hover {
    background: #888888;
}
</style>
