import React, { Component } from "react";
import { Link } from "react-router-dom";
import { Input } from "../Input";
import axios from "axios";
import API_ADDRESS from "../../API_ADDRESS";
import validator from "validator";
import { toast } from "react-toastify";
import { Container, Tab } from "./Form.styles.jsx";

export class Employer extends Component {
  state = {
    firstName: "",
    lastName: "",
    phoneNumber: "",
    isValid: true,
    role: "",
    firstNameError: null,
    lastNameError: null,
    phError: null,
  };


  componentDidMount() {
    const role = this.props.prop.match.params.role;
    this.setState({ role });
  }

  submitHandler = async (event) => {
    await event.preventDefault();

    const { phoneNumber, firstName, lastName } = this.state;
    console.log(firstName, lastName)
    if (!phoneNumber || !firstName || !lastName) {
      toast.error("برای ثبت نام باید همه اطلاعات خواسته شده را وارد کنید .");
      return;
    }
    else if (!phoneNumber.startsWith("09")) {
      toast.error("شماره تلفن باید با 09 شروع شود.");
      return;
    }
    else if (phoneNumber.length !== 11) {
      toast.error("شماره تلفن باید 11 رقم باشد.");
      return;
    }
    else if (this.state.isValid) {
      await axios
        .post(API_ADDRESS + "Account/EmployerRegister", {
          phoneNumber: this.state.phoneNumber,
          fullName: `${this.state.firstName} ${this.state.lastName}`,
        })
        .then((res) => {
          if (res.status === 200) {
            
              this.props.prop.history.push(
                `/Employer/Register/Verification?phoneNumber=${this.state.phoneNumber}`
              );
          }
        })
        .catch((err) => {
          err.response.data.message &&
            err.response.data.message.map((er) => toast.error(er));
        });
    }
  };

  changeHandler = (e, name) => {
    let val = e.target.value;


    if (name === 'phoneNumber') {


      if (val.match(/[۰]/g)) {
        val = val.replace(/[۰]/g, '0');
      }
      if (val.match(/[۱]/g)) {
        val = val.replace(/[۱]/g, '1');
      }
      if (val.match(/[۲]/g)) {
        val = val.replace(/[۲]/g, '2');
      }
      if (val.match(/[۳]/g)) {
        val = val.replace(/[۳]/g, '3');
      }
      if (val.match(/[۴]/g)) {
        val = val.replace(/[۴]/g, '4');
      }
      if (val.match(/[۵]/g)) {
        val = val.replace(/[۵]/g, '5');
      }
      if (val.match(/[۶]/g)) {
        val = val.replace(/[۶]/g, '6');
      }
      if (val.match(/[۷]/g)) {
        val = val.replace(/[۷]/g, '7');
      }
      if (val.match(/[۸]/g)) {
        val = val.replace(/[۸]/g, '8');
      }
      if (val.match(/[۹]/g)) {
        val = val.replace(/[۹]/g, '9');
      }
      if (val.match(/[^0123456789]/g)) {
        val = val.replace(/[^0123456789]/g, '');
      }

      this.setState({ phoneNumber: val });

    }

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
            <form className="w-100" noValidate onSubmit={this.submitHandler}>
              <div
                className="bg-white sp-2 smb-2"
                style={{ borderRadius: "0 0 10px 10px" }}
              >
                <h1 className="fs-l c-dark d-block text-center smb-5 ir-bl">
                  ثبت نام کارفرمایان
                </h1>


                <label htmlFor="firstName" className="m-0 mr-1 mt-2 p-0 mb-2 d-flex flex-row justify-content-start align-items-baseline">

                  <span className="m-0 p-0 ir-r">لطفا نام خود را وارد کنید</span>
                  <span style={{ color: "red" }} className="m-0 mr-1 mt-1 text-danger">*</span>
                </label>
                <input id="firstName" placeholder="نام " type="text" className="w-100 m-0 mb-2 mt-0 py-2 px-3 text-right ir-r" value={this.state.firstName} onChange={(e) => { this.setState({ firstName: e.target.value }) }}
                  style={{ border: "1px solid #9999", borderRadius: "10px", outline: "none" }} />




                <label htmlFor="lastName" className="m-0 mr-1 mt-2 p-0 mb-2 d-flex flex-row justify-content-start align-items-baseline">

                  <span className="m-0 p-0 ir-r">لطفا نام خانوادگی خود را وارد کنید</span>
                  <span style={{ color: "red" }} className="m-0 mr-1 mt-1 text-danger">*</span>
                </label>
                <input id="lastName" placeholder="نام خانوادگی" type="text" className="w-100 m-0  mb-2 mt-0 py-2 px-3 text-right ir-r" value={this.state.lastName} onChange={(e) => { this.setState({ lastName: e.target.value }) }}
                  style={{ border: "1px solid #9999", borderRadius: "10px", outline: "none" }} />




                <label htmlFor="phNumber" className="m-0 mr-1 mt-2 p-0 mb-2 d-flex flex-row justify-content-start align-items-baseline">

                  <span className="m-0 p-0 ir-r"> لطفا شماره تلفن همراه خود را با 09 وارد کنید</span>
                  <span style={{ color: "red" }} className="m-0 mr-1 mt-1 text-danger">*</span>
                </label>
                <input id="phNumber" placeholder="مثال: 09111111111" type="text" maxLength="11" className="w-100 m-0 mb-2 mt-0 py-2 px-3 text-right ir-r" value={this.state.phoneNumber} onChange={(e) => { this.changeHandler(e, "phoneNumber") }}
                  style={{ border: "1px solid #9999", borderRadius: "10px", outline: "none" }} />


                {this.state.phError &&
                  <div className="ir-r badge badge-danger m-0 p-2 d-flex flex-row justify-content-start align-items-stretch">
                    {this.state.phError}
                  </div>}


                <footer className="d-flex justify-content-between align-items-center smt-2">
                  <Link className="ir-r text-primary" to="/Employer/Login">
                    ورود کارفرمایان
                  </Link>

                  <button type="submit" className="btn btn-warning ir-r">
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
