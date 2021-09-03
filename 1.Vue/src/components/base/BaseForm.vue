<template>
  <div
    :class="['container', { open: isOpen, close: !isOpen }]"
    id="container"
    @click="closeForm(isChange)"
  >
    <div
      :class="['form-container', { open: isOpen, close: !isOpen }]"
      id="form-container"
      @click.stop="formClick"
      ref="draggableContainer"
    >
      <div
        class="form-header"
        id="form-container-header"
        @mousedown="this.dragMouseDown"
      >
        <div class="title-header">
          <div class="form-title">{{ $resourceVn.EmployeeContentHeader }}</div>
          <div class="options">
            <div class="option" @click="detail.IsCustomer = !detail.IsCustomer">
              <span class="checkbox-container">
                <input
                  type="checkbox"
                  name="isCustomer"
                  v-model="detail.IsCustomer"
                />
                <div
                  class="checkmark"
                  :tabindex="1"
                  @keyup.enter="detail.IsCustomer = !detail.IsCustomer"
                ></div>
              </span>
              <span class="label">{{ $resourceVn.IsCustomer }}</span>
            </div>
            <div class="option" @click="detail.IsProvider = !detail.IsProvider">
              <span class="checkbox-container">
                <input
                  type="checkbox"
                  name="isProvider"
                  v-model="detail.IsProvider"
                />
                <div
                  class="checkmark"
                  :tabindex="2"
                  @keyup.enter="detail.IsProvider = !detail.IsProvider"
                ></div>
              </span>
              <span class="label">{{ $resourceVn.IsProvider }}</span>
            </div>
          </div>
        </div>
        <div class="header-btn">
          <div
            class="help-btn form-header-btn"
            :title="$resourceVn.HelpButtonTitle"
          ></div>
          <div
            class="close-btn form-header-btn"
            :title="$resourceVn.CloseButtonTitle"
            @click="closeForm(isChange)"
          ></div>
        </div>
      </div>
      <div class="form-body">
        <!-- secion 1 -->
        <div class="form-section form-section-1">
          <!-- Column 1 -->
          <div class="section-column section-column-1">
            <div class="input-row">
              <BaseTextInput
                :valueType="'text'"
                type="input-form"
                inputKey="employee-code"
                :tabindex="3"
                ref="employeeCode"
                v-model="detail.EmployeeCode"
                :label="$resourceVn.LabelEmployeeCode"
                :required="true"
                :validates="[required]"
                :rerenderFlag="isRerender"
                @valid="validateForm"
              />
              <BaseTextInput
                v-model="detail.FullName"
                :valueType="'text'"
                type="input-form"
                inputKey="fullname"
                :tabindex="4"
                :label="$resourceVn.LabelFullname"
                :required="true"
                ref="fullName"
                :validates="[required]"
                :rerenderFlag="isRerender"
                @valid="validateForm"
              />
            </div>
            <div class="input-row">
              <div class="input-field" id="unit">
                <div class="input-label">
                  {{ $resourceVn.LabelUnit }}
                  <span v-if="required"><span class="required">*</span></span>
                </div>

                <Combobox
                  :mode="0"
                  :api="`${$config.BASE_API}/Units`"
                  :type="'Unit'"
                  :typeDataKey="'UnitName'"
                  :tabindex="5"
                  :label="$resourceVn.LabelUnit"
                  :validates="[required]"
                  @itemChange="dropDataChange"
                  ref="unitName"
                  @valid="validateForm"
                  v-model="detail.UnitName"
                  :rerenderFlag="isRerender"
                >
                </Combobox>
              </div>
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.PositionName"
                :valueType="'text'"
                type="input-form"
                inputKey="position"
                :tabindex="6"
                :label="$resourceVn.LabelPositionName"
              />
            </div>
          </div>
          <!-- Column 2 -->
          <div class="section-column section-column-2">
            <div class="input-row">
              <BaseDateInput
                :type="'input-form'"
                inputKey="date-of-birth"
                ref="dateOfBirth"
                :tabindex="7"
                v-model="detail.DateOfBirth"
                :label="$resourceVn.LabelDateOfBirth"
                :validates="[date, dateNotExceedToday]"
                :rerenderFlag="isRerender"
                @valid="validateForm"
                @dateChange="dateChange"
              />
              <div class="input-field" id="gender">
                <div class="input-label">{{ $resourceVn.LabelGender }}</div>

                <div class="input">
                  <label class="icontainer">
                    {{ $resourceVn.OptionMale }}
                    <input
                      :tabindex="8"
                      type="radio"
                      value="1"
                      name="gender"
                      v-model="detail.Gender"
                    />
                    <span class="radio-checkmark"></span>
                  </label>
                  <label class="icontainer">
                    {{ $resourceVn.OptionFemale }}
                    <input
                      :tabindex="9"
                      type="radio"
                      value="0"
                      name="gender"
                      v-model="detail.Gender"
                    />
                    <span class="radio-checkmark"></span>
                  </label>
                  <label class="icontainer">
                    {{ $resourceVn.OptionOthers }}
                    <input
                      :tabindex="10"
                      type="radio"
                      value="2"
                      name="gender"
                      v-model="detail.Gender"
                    />
                    <span class="radio-checkmark"></span>
                  </label>
                </div>
              </div>
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.IdentityNumber"
                :valueType="'text'"
                type="input-form"
                inputKey="identity-number"
                :tabindex="11"
                :label="$resourceVn.LabelIdentityNumber"
                :rerenderFlag="isRerender"
                :pattern="/[0-9]/"
              />
              <BaseDateInput
                v-model="detail.IdentityDate"
                :type="'input-form'"
                inputKey="identity-date"
                :tabindex="12"
                :label="$resourceVn.LabelIdentityDate"
                :validates="[date, dateNotExceedToday]"
                :rerenderFlag="isRerender"
                @dateChange="dateChange"
                @valid="validateForm"
                ref="identityDate"
              />
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.IdentityPlace"
                :valueType="'text'"
                type="input-form"
                inputKey="identity-place"
                :tabindex="13"
                :label="$resourceVn.LabelIdentityPlace"
              />
            </div>
          </div>
        </div>
        <div class="form-section form-section-2">
          <div class="">
            <div class="input-row">
              <BaseTextInput
                v-model="detail.Address"
                :valueType="'text'"
                type="input-form"
                inputKey="address"
                :tabindex="14"
                :label="$resourceVn.LabelAddress"
              />
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.MobileNumber"
                :valueType="'text'"
                type="input-form"
                inputKey="mobile-number"
                :tabindex="15"
                :label="$resourceVn.LabelMobileNumber"
                ref="mobileNumber"
                :pattern="/[0-9]/"
              />
              <BaseTextInput
                v-model="detail.PhoneNumber"
                :valueType="'text'"
                type="input-form"
                inputKey="phone-number"
                :tabindex="16"
                :label="$resourceVn.LabelPhoneNumber"
                ref="phoneNumber"
                :pattern="/[0-9()-]/"
              />
              <BaseTextInput
                v-model="detail.Email"
                :valueType="'text'"
                type="input-form"
                inputKey="email"
                :tabindex="17"
                :label="$resourceVn.LabelEmail"
                ref="email"
                :rerenderFlag="isRerender"
                :validates="[email]"
                @valid="validateForm"
              />
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.BankAccount"
                :valueType="'text'"
                type="input-form"
                inputKey="bank-account"
                :tabindex="18"
                :label="$resourceVn.LabelBankAccount"
                :pattern="/[0-9]/"
              />
              <BaseTextInput
                v-model="detail.BankName"
                :valueType="'text'"
                type="input-form"
                inputKey="bank-name"
                :tabindex="19"
                :label="$resourceVn.LabelBankName"
              />
              <BaseTextInput
                v-model="detail.BankBranch"
                :valueType="'text'"
                type="input-form"
                inputKey="bank-branch"
                :tabindex="20"
                :label="$resourceVn.LabelBankBranch"
              />
            </div>
          </div>
        </div>
      </div>

      <div class="form-footer">
        <!-- <div class="button  cancel-btn">Hủy</div> -->
        <BaseButtonIcon
          :value="$resourceVn.CancelButtonText"
          class="form-btn"
          :type="'cancel-btn'"
          :onclick="
            () => {
              closeForm(isChange);
            }
          "
          tabindex="21"
        ></BaseButtonIcon>
        <div class="action-btns">
          <BaseButtonIcon
            class="form-btn"
            :value="$resourceVn.SaveButtonText"
            :type="'save-btn'"
            :onclick="
              () => {
                btnSaveClick(false);
              }
            "
            tabindex="22"
          ></BaseButtonIcon>
          <BaseButtonIcon
            class="form-btn"
            :value="$resourceVn.SaveAddButtonText"
            :type="'save-add-btn'"
            :onclick="
              () => {
                btnSaveClick(true);
              }
            "
            tabindex="23"
          ></BaseButtonIcon>
        </div>
      </div>
      <div id="loader" class="spinner-wrapper" :class="{ hidden: !isLoading }">
        <div class="spinner"></div>
      </div>
    </div>
  </div>
</template>

<script>
import EventBus from "../../event-bus/EventBus";
import axios from "axios";
import ultis from "../../mixins/ultis";
import validate from "../../mixins/validate";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
import BaseTextInput from "../base/BaseTextInput.vue";
import BaseDateInput from "../base/BaseDateInput.vue";
import Combobox from "../base/BaseCombobox.vue";

const initObject = {
  IsCustomer: false,
  IsProvider: false,
  Gender: 2,
};

export default {
  name: "Form",
  components: {
    BaseButtonIcon,
    BaseTextInput,
    BaseDateInput,
    Combobox,
  },
  mixins: [ultis, validate],
  props: {
    isOpen: {
      type: Boolean,
      required: true,
    },
    close: {
      type: Function,
      require: false,
    },
    mode: {
      type: Number,
      required: false,
      default() {
        return 0;
      }, // 0: For POST action, 1: For PUT action
    },
    detailId: {
      type: String,
      required: false,
    },
    moreDetail: {
      type: Object,
      required: false,
    },
  },
  data() {
    return {
      detail: {},
      initDetail: {},
      isDetailChange: false,
      isDataLoaded: false,
      isRerender: false,
      validate: {
        "employee-code": false,
        fullname: false,
        UnitName: false,
        email: false,
        "date-of-birth": false,
        "identity-date": false,
      },
      validateRefs: [
        "employeeCode",
        "fullName",
        "dateOfBirth",
        "unitName",
        "identityDate",
        "phoneNumber",
        "mobileNumber",
        "email",
      ],
      dateMap: {
        "date-of-birth": "DateOfBirth",
        "identity-date": "IdentityDate",
      },
      dateName: {
        "date-of-birth": "Ngày sinh",
        "identity-date": "Ngày cấp",
      },
      dateInputFormat: "yyyy-MM-dd",
      isLoading: false,
      positions: {
        clientX: undefined,
        clientY: undefined,
        movementX: 0,
        movementY: 0,
      },
    };
  },
  mounted() {},
  computed: {
    isChange: function () {
      return (
        Object.entries(this.initDetail).toString() !=
        Object.entries(this.detail).toString()
      );
    },
  },
  watch: {
    /**
     * Watch sự thay đổi của isOpen để
     * - Auto focus vào trường mã nhân viên khi mở form
     * - Gọi API lấy thông tin đối tượng nếu form mode = 0 <sửa>
     * CreatedBy: HungNguyen81 (07-2021)
     */
    isOpen: function (val) {
      // khởi tạo các giá trị ban đầu => so sánh khi đóng form
      if (
        this.mode == this.$config.FORM_ADD ||
        this.mode == this.$config.FORM_CLONE
      ) {
        this.initDetail = Object.assign({}, initObject);
        this.detail = Object.assign({}, initObject);
      }

      this.isDataLoaded = false;
      this.isRerender = !this.isRerender;

      console.log("form " + (val ? "open" : "close"), this.mode);

      if (this.isOpen) {
        this.$refs.draggableContainer.style.top = "50px";
        this.$refs.draggableContainer.style.left = "50%";

        // Nếu mode là FORM_UPDATE: sửa nv
        if (this.mode == this.$config.FORM_UPDATE && this.detailId) {
          this.generateUpdateForm();
        }
        // Nếu mode là FORM_ADD: thêm mới nv
        else if (this.mode == this.$config.FORM_ADD) {
          this.generateAddForm();
        } else if (this.mode == this.$config.FORM_CLONE) {
          this.generateUpdateForm(this.getNewCode);
        }
      }

      if (this.isOpen) {
        document.addEventListener("keydown", this.escListener);
      } else {
        document.removeEventListener("keydown", this.escListener);
      }
    },
    isLoading: function (state) {
      if (!state) {
        this.$nextTick(() => {
          if (this.isOpen) this.$refs.employeeCode.$el.children[1].focus();
        });
      }
    },
  },
  methods: {
    /**
     * Đóng form khi bấm phím Esc trên bàn phím
     * CreatedBy: HungNguyen81 (03-09-2021)
     */
    escListener(e) {
      if (e.key == "Escape") {
        if (!this.isLoading) this.closeForm(this.isChange);
      }
    },

    /**
     * Đóng form, gọi hàm Close truyền từ props
     * CreatedBy: HungNguyen81 (08-2021)
     */
    closeForm(isChange, event) {
      if (this.isLoading) {
        event.preventDefault();
      }
      this.close(isChange, this.mode, this.detailId, this.getRawData());
    },

    /**
     * Handle sự kiện click trên form để đóng các dropdown list đang mở
     * CreatedBy: HungNguyen81 (09-2021)
     */
    formClick(event) {
      EventBus.$emit("appClick", event.target);
    },

    /**
     * Khởi tạo data cho form với mode = FORM_UPDATE
     * CreatedBy: HungNguyen81 (30-08-2021)
     */
    generateUpdateForm: function (callback) {
      this.isLoading = true;
      axios
        .get(`${this.$config.BASE_API}/Employees/${this.detailId}`)
        .then((res) => {
          if (res.data.IsValid === false) {
            this.$emit("showToast", "warning", "NO Content", res.data.Msg);
            return;
          }

          this.detail = Object.assign({}, res.data);
          console.table(res.data);

          console.groupCollapsed("Data form");
          console.table(res.data);
          console.groupEnd();

          this.formatData();

          this.initDetail = Object.assign({}, this.detail);
          this.isDataLoaded = true;
          this.isLoading = false;

          if (callback) callback();
        })
        .catch((err) => {
          // Handle khi không có kết nối Internet
          if (!err.response) {
            this.$emit("dataLoaded");
            this.$emit(
              "showToast",
              "error",
              this.$resourceVn.ErrorTitle,
              this.$resourceVn.NetworkErrorMsg
            );
            this.isLoading = false;
            return;
          }

          this.$emit(
            "showToast",
            "error",
            this.$resourceVn.ErrorTitle,
            this.$resourceVn.ServerErrorMsg
          );
          this.isLoading = false;
        });
    },

    /**
     * Khởi tạo form với mode = FORM_ADD
     * CreatedBy: HungNguyen81 (30-08-2021)
     */
    generateAddForm: function () {
      this.isDataLoaded = true;

      this.getNewCode();
    },

    /**
     * Lấy mã nv mới từ API
     * CreatedBy: HungNguyen81 (30-08-2021)
     */
    getNewCode() {
      this.isLoading = true;
      axios
        .get(`${this.$config.BASE_API}/Employees/NewEmployeeCode`)
        .then((res) => {
          let newCode = res.data.Data;
          this.$refs.employeeCode.$el.value = newCode;
          if (!newCode) {
            this.$emit(
              "showToast",
              "error",
              this.$resourceVn.ErrorTitle,
              this.$resourceVn.CannotGetNewEmployeeCodeMsg
            );
          }
          this.$set(this.detail, "EmployeeCode", newCode);

          // this.initDetail = Object.assign({}, this.detail);
          this.isLoading = false;
        })
        .catch(() => {
          this.$emit(
            "showToast",
            "error",
            this.$resourceVn.ErrorTitle,
            this.$resourceVn.CannotGetNewEmployeeCodeMsg
          );
          this.isLoading = false;
        });
    },

    /**
     * Handle mỗi khi date input value thay đổi
     * CreatedBy: HungNguyen81 (07-2021)
     */
    dateChange(key, val, input, formatedVal) {
      let keyName = this.dateMap[key];
      let oldVal = this.detail[keyName];
      if (oldVal != val && val) {
        // validate định dạng ngày
        !this.date(this.dateName[key], formatedVal);

        let start = input.selectionStart;
        // console.log("start", start, ", date", val, formatedVal);
        this.$set(this.detail, keyName, val);
        this.$nextTick(() => {
          input.setSelectionRange(start + 1, start + 1);
        });
      }
    },

    /**
     * Format dữ liệu để hiển thị
     * CreatedBy: HungNguyen81 (07-2021)
     */
    formatData() {
      this.$set(
        this.detail,
        "DateOfBirth",
        this.dateFormatVer2(this.detail.DateOfBirth, this.dateInputFormat)
      );
      this.$set(
        this.detail,
        "IdentityDate",
        this.dateFormatVer2(this.detail.IdentityDate, this.dateInputFormat)
      );
      if (this.detail.Gender === null) {
        this.$set(this.detail, "Gender", 2);
      }
    },

    /**
     * Kiểm tra tính hợp lệ của các trường dữ liệu trên form
     * CreatedBy: HungNguyen81 (07-2021)
     */
    isValidate() {
      let res = true;
      Object.keys(this.validate).forEach((key) => {
        res = res && this.validate[key];
      });
      console.groupCollapsed("Validate info");
      console.table(JSON.parse(JSON.stringify(this.validate)));
      console.groupEnd();
      return res;
    },

    /**
     * Lấy dữ liệu thô để post/put
     * CreatedBy: HungNguyen81 (07-2021)
     */
    getRawData() {
      let dob = this.detail.DateOfBirth;
      let identityDate = this.detail.IdentityDate;
      let res = JSON.parse(JSON.stringify(this.detail));

      res.DateOfBirth = dob ? new Date(dob).toISOString() : null;
      res.IdentityDate = identityDate
        ? new Date(identityDate).toISOString()
        : null;
      res.Gender = Number(this.detail.Gender);
      return res;
    },

    /**
     * Handle khi click nút lưu
     * CreatedBy: HungNguyen81 (07-2021)
     */
    btnSaveClick(isAddNext) {
      this.isLoading = true;

      var invalidMsg = "";
      var isInvalid = false;
      for (let ref of this.validateRefs) {
        let valid = this.$refs[ref].inputValidate();

        // lấy thông báo ko hợp lệ đầu tiên để hiện thị lên
        if (!valid.IsValid && !isInvalid) {
          invalidMsg = valid.Msg;
          isInvalid = true;
        }
      }
      if (!this.isValidate()) {
        this.$emit(
          "showToast",
          "warning",
          this.$resourceVn.DataInvalidMsg,
          invalidMsg
        );
        this.$emit("showPopup", {
          content: invalidMsg,
          popupType: "error",
          isHide: false,
          buttons: [
            {
              type: "button-ok",
              callback: null,
              value: this.$resourceVn.CloseButtonText,
            },
          ],
        });

        this.isLoading = false;
        return;
      }

      var func = () => {
        this.initDetail = Object.assign({}, initObject);
        this.detail = Object.assign({}, initObject);
        this.getNewCode();

        // this.isLoading = false;
      };

      // nếu thông tin đã thay đổi
      if (this.isChange) {
        this.$emit(
          "saveClicked",
          this.mode,
          this.detailId,
          this.getRawData(),
          isAddNext
            ? func
            : () => {
                this.isLoading = false;
                this.closeForm(false);
              }
        );
        EventBus.$on("requestFail", () => {
          this.isLoading = false;
        });
      } else {
        if (!isAddNext) {
          this.isLoading = false;
          this.closeForm(false);
        } else {
          func();
        }
      }
    },

    /**
     * Khi select trong dropdown, chỉ có DepartmentName thay đổi mà DepartmentId không thay đổi
     * Tương tự với PositionId
     * CreatedBy: HungNguyen81 (07-2021)
     */
    dropDataChange(typeName, obj) {
      this.$set(this.detail, typeName, obj[typeName]);

      var key = typeName.split("Name")[0] + "Id";

      if (obj[key]) {
        this.$set(this.detail, key, obj[key]);
      }
    },

    /**
     * Emit sự kiện showToast cho parent(base content)
     * CreatedBy: HungNguyen81 (07-2021)
     */
    emitShowToast(type, header, msg) {
      this.$emit("showToast", type, header, msg);
    },

    /**
     * Handle khi validate input
     * CreatedBy: HungNguyen81 (07-2021)
     */
    validateForm(key, isValid) {
      this.$set(this.validate, key, isValid);
    },

    //#region Tạo Draggable form

    dragMouseDown: function (event) {
      event.preventDefault();
      // lấy vị trí của con trỏ chuột
      this.positions.clientX = event.clientX;
      this.positions.clientY = event.clientY;
      document.onmousemove = this.elementDrag;
      document.onmouseup = this.closeDragElement;
    },
    elementDrag: function (event) {
      event.preventDefault();
      this.positions.movementX = this.positions.clientX - event.clientX;
      this.positions.movementY = this.positions.clientY - event.clientY;
      this.positions.clientX = event.clientX;
      this.positions.clientY = event.clientY;

      // set vị trí mới cho element
      this.$refs.draggableContainer.style.top =
        this.$refs.draggableContainer.offsetTop -
        this.positions.movementY +
        "px";
      this.$refs.draggableContainer.style.left =
        this.$refs.draggableContainer.offsetLeft -
        this.positions.movementX +
        "px";
    },
    closeDragElement() {
      document.onmouseup = null;
      document.onmousemove = null;
    },

    //#region
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
@import "../../css/base/checkbox.css";
@import "../../css/base/radio.css";
@import "../../css/base/loader.css";
</style>