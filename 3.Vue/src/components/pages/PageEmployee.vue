<template>
  <div>
    <Header :collapse="collapse" :class="{collapse: isCollapse}"></Header>
    <Menu :route="route" :items="menuItems" :class="{collapse: isCollapse}"></Menu>
    <Content
      :features="features"
      entityName="Employee"
      :filterNames="['DepartmentId', 'PositionId']"
      :headers="headers"
      :class="{collapse: isCollapse}"
    ></Content>
  </div>
</template>

<script>
import Header from "../layout/TheHeader.vue";
import Menu from "../layout/TheMenu.vue";
import Content from "../base/BaseContent.vue";
import axios from "axios";
import ultis from '../../mixins/ultis'

export default {
  name: "EmployeePage",
  components: {
    Header,
    Menu,
    Content,
  },
  mixins:[ultis],
  data() {
    return {
      headers: this.$resourceVn.TableHeaders,
      features: [],
      menuItems: this.$resourceVn.MenuItems,
      isCollapse: false,
      route: "/",
    };
  },
  created() {
    this.features.push({
      featureClass: "btn-export",
      action: this.exportEmployees,
    });
  },
  methods: {
    /**
     * Xuất dữ liệu toàn bộ nv ra file excel
     * CreatedBy: HungNguyen81 (31-08-2021)
     */
    exportEmployees() {
      // var dateFormat = "yyyyMMdd-hhmmss";
      var currTimeString = this.dateFormatVer2(new Date(), "yyyyMMddhhmmss");
      document.querySelector('.btn-export').classList.toggle('loading');
      axios
        .get(`${this.$config.BASE_API}/Employees/employeesFile`,{
          responseType: 'blob',
        })
        .then(response => {
          document.querySelector('.btn-export').classList.toggle('loading');
          const url = URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute(
            "download",
            `${this.$resourceVn.ExportFilePrefix}_${currTimeString}.xlsx`
          );
          document.body.appendChild(link);
          link.click();
        });
    },
    collapse(){
      this.isCollapse = !this.isCollapse;
    }
  },
};
</script>

<style>
</style>
