import React, { Component } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import API_ADDRESS from "../../API_ADDRESS";
import validator from "validator";
import { toast } from "react-toastify";
import { Container, Tab } from "./Form.styles.jsx";

export class Employee extends Component {
  state = {
    fullName: "",
    phoneNumber: "",
    firstName: '',
    lastName: '',
    errors: { fullName: "", phoneNumber: "", firstName: "", lastName: "" },
  };

  

  async changeHandler(event) {
    const formData = { [event.target.name]: event.target.value };
    await this.setState({ ...this.state, ...formData });
  }
  componentDidMount() {
    
    const role = this.props.prop.match.params.role;
    this.setState({ role });

  }
  submitHandler(event) {
    event.preventDefault();

    const { phoneNumber, firstName, lastName } = this.state;
    if (!phoneNumber || !firstName || !lastName) {
      toast.error("برای ثبت نام باید همه اطلاعات خواسته شده را وارد کنید .");
      return;
    }
    else if (this.formIsValid()) {
      axios
        .post(API_ADDRESS + "Account/EmployeeRegister", {
          fullName: `${this.state.firstName} ${this.state.lastName}`,
          phoneNumber: this.state.phoneNumber.toString(),
        })
        .then((res) => {
          console.log(res.data)
          alert(res.data.resul)
          if (res.status === 200) {
            if (this.state.adverId) {
              this.props.prop.history.push(`/JobDetails/${this.state.adverId}`);
            }
            else
              this.props.prop.history.push(
                `/Employee/Register/Verification?phoneNumber=${res.data.resul}`
              );
          }

        })
        .catch((err) => {
          err.response.data.message &&
            err.response.data.message.map((er) => toast.error(er));

          this.setState({
            ...this.state,
            errors: {
              ...this.state.errors,
              phoneNumber: err.response.data.message,
            },
          });
        });
    }
  }

  formIsValid() {
    let errors = {};


    let fnLength = (this.state.firstName.length + this.state.lastName.length) >= 6 ? true : false;

    let phLength = this.state.phoneNumber?.length === 11 ? true : false;

    if (!phLength) errors.phoneNumber = "شماره موبایل باید 11 رقم باشد.";

    this.setState({ ...this.state.errors, errors });

    return (errors.phoneNumber || fnLength);
  }


  async lastNameValidator() {
    const lastNameValue = await validator.isEmpty(this.state.lastName);

    const fnLang = await validator.isAlpha(this.state.lastName, [
      "fa-IR",
    ]);

    if (lastNameValue) {
      const newErrors = { ...this.state.errors };
      newErrors.lastName = true;
      await this.setState({ errors: newErrors });
    } else {
      const newErrors = { ...this.state.errors };
      newErrors.lastName = false;
      await this.setState({ errors: newErrors });
    }

    if (!fnLang) {
      const newErrors = { ...this.state.errors };
      newErrors.lastName = false;
      await this.setState({ errors: newErrors });
    } else {
      const newErrors = { ...this.state.errors };
      newErrors.lastName = true;
      await this.setState({ errors: newErrors });
    }
  }

  async firstNameValidator() {
    const fisrtName = await validator.isEmpty(this.state.firstName);

    const fnLang = await validator.isAlpha(this.state.firstName, [
      "fa-IR",
    ]);

    if (fisrtName) {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = true;
      await this.setState({ errors: newErrors });
    } else {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = false;
      await this.setState({ errors: newErrors });
    }

    if (!fnLang) {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = false;
      await this.setState({ errors: newErrors });
    } else {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = true;
      await this.setState({ errors: newErrors });
    }
  }


  async phValidator() {
    const phValue = await validator.isEmpty(this.state.phoneNumber);
    const phNumber = await validator.isMobilePhone(
      this.state.phoneNumber,
      ["fa-IR"]
    );

    if (phValue) {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = true;
      await this.setState({ errors: newErrors });
    } else {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = false;
      await this.setState({ errors: newErrors });
    }

    if (!phNumber) {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = true;
      await this.setState({ errors: newErrors });
    } else {
      const newErrors = { ...this.state.errors };
      newErrors.fisrtName = false;
      await this.setState({ errors: newErrors });
    }
  }


  isInputValid(e) {
    let val = e.target?.value;
    let phonenumber = e.target?.value
    if (phonenumber.match(/[۰]/g)) {
      phonenumber = phonenumber.replace(/[۰]/g, '0');
    }
    if (phonenumber.match(/[۱]/g)) {
      phonenumber = phonenumber.replace(/[۱]/g, '1');
    }
    if (phonenumber.match(/[۲]/g)) {
      phonenumber = phonenumber.replace(/[۲]/g, '2');
    }
    if (phonenumber.match(/[۳]/g)) {
      phonenumber = phonenumber.replace(/[۳]/g, '3');
    }
    if (phonenumber.match(/[۴]/g)) {
      phonenumber = phonenumber.replace(/[۴]/g, '4');
    }
    if (phonenumber.match(/[۵]/g)) {
      phonenumber = phonenumber.replace('۵', '5');
    }
    if (phonenumber.match(/[۶]/g)) {
      phonenumber = phonenumber.replace(/[۶]/g, '6');
    }
    if (phonenumber.match(/[۷]/g)) {
      phonenumber = phonenumber.replace(/[۷]/g, '7');
    }
    if (phonenumber.match(/[۸]/g)) {
      phonenumber = phonenumber.replace(/[۸]/g, '8');
    }
    if (phonenumber.match(/[۹]/g)) {
      phonenumber = phonenumber.replace(/[۹]/g, '9');
    }

    if (phonenumber.match(/[^0123456789۰۱۲۳۴۵۶۷۸۹]/g)) {
      phonenumber = phonenumber.replace(/[^0123456789۰۱۲۳۴۵۶۷۸۹]/g, '');
    }

    this.setState({
      [e.target.name]: phonenumber
    })
    // this.setState({ phonenumber })
  };


  writeEnglishAlert(e) {
    let val = e.target?.value;
    if (val?.match(/[۰۱۲۳۴۵۶۷۸۹]/g)) {
      toast.error('لطفا اعداد را به صورت لاتین وارد کنید')
    }

    if (e.target.value?.match(/[^0-9]/g)) {
      val = val.replace(/[^0-9]/g, '');
    }
    this.setState({
      [e.target.name]: val
    })
  }


  render() {
    return (
      <section className="container-fluid spx-2 spx-lg-10 smy-10 spt-10">
        <div className="row">
          <aside className="col-12 col-lg-5 mx-auto">
            <Container>
              <Tab
                active={this.state.role === "Employer" && "#ffc107"}
                to="/Employer/Login"
              >
                کارفرمایان
              </Tab>
              <Tab
                active={this.state.role === "Employee" && "#007bff"}
                to="/Employee/Login"
              >
                کارجویان
              </Tab>
            </Container>
            <form
              noValidate
              onSubmit={this.submitHandler.bind(this)}
              className="w-100"
            >
              <div
                className="bg-white sp-2 smb-2"
                style={{ borderRadius: "0 0 10px 10px" }}
              >
                <h1 className="fs-l c-dark d-block text-center smb-5 ir-bl">
                  ثبت نام کارجویان
                </h1>

                <div className="text-input srounded-sm">
                  <label
                    className="ir-r text-regular text-right smb-1 label bg-white"
                    htmlFor="fullName"
                  >
                    لطفا نام خود را وارد کنید
                    <span className="text-danger d-inline">*</span>
                  </label>

                  <div className="form-group d-flex justify-content-center align-items-center">
                    <input
                      name="firstName"
                      onChange={this.changeHandler.bind(this)}
                      value={this.state.firstName || ""}
                      id="firstName"
                      className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
                      type="text"
                      placeholder="نام"
                    />
                  </div>
                </div>
                <span className="d-block c-danger fs-s ir-r smb-3">
                  {this.state.errors.firstName}
                </span>

                <div className="text-input srounded-sm">
                  <label
                    className="ir-r text-regular text-right smb-1 label bg-white"
                    htmlFor="lastName"
                  >
                    لطفا نام خانوادگی خود را وارد کنید
                    <span className="text-danger d-inline">*</span>
                  </label>

                  <div className="form-group d-flex justify-content-center align-items-center">
                    <input
                      name="lastName"
                      onChange={this.changeHandler.bind(this)}
                      value={this.state.lastName || ""}
                      id="lastName"
                      className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
                      type="text"
                      placeholder=" نام خانوادگی"
                    />
                  </div>
                </div>

                <span className="d-block c-danger fs-s ir-r smb-2">
                  {this.state.errors.lastName}
                </span>

                <div className="text-input srounded-sm">
                  <label
                    className="ir-r text-regular text-right smb-1 label bg-white"
                    htmlFor="phoneNumber"
                  >
                    لطفا شماره موبایل خود را با 09 وارد کنید
                    <span className="text-danger d-inline">*</span>
                  </label>

                  <div className="form-group d-flex justify-content-center align-items-center">
                    <input
                      name="phoneNumber"
                      onKeyUp={(e) => { this.isInputValid(e) }}
                      onKeyPress={(e) => { this.isInputValid(e) }}
                      onKeyDown={(e) => { this.isInputValid(e) }}
                      onChange={(e) => { this.isInputValid(e) }}
                      value={this.state.phoneNumber}
                      id="phoneNumber"
                      maxLength="11"
                      minLength="11"
                      className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
                      type="text"
                      placeholder="مثال: 09111111111"
                    />
                  </div>
                </div>

                <span className="d-block c-danger fs-s ir-r smb-2">
                  {this.state.errors.phoneNumber}
                </span>

                <footer className="d-flex justify-content-between align-items-center smt-2">
                  <Link className="ir-r text-primary" to="/Employee/Login">
                    ورود کارجویان
                  </Link>

                  <button type="submit" className="btn btn-primary ir-r">
                    ارسال کد تایید
                  </button>
                </footer>
              </div>
            </form>
          </aside>
        </div>
      </section>
    );
  }
}
