import React, { Component } from "react";
import { Input } from "../components";
import API_ADDRESS from "../API_ADDRESS";
import axios from "axios";
import Timer from "react-compound-timer";
import validator from "validator";
import auth from "../core/authService";
import { Redirect } from "react-router-dom";
export class Verification extends Component {
  state = {
    verification: {
      phoneNumber: "",
      verifyCode: "",
      timer: 120000,
      refresh: false,
      reset: false,
    },

    error: "",
  };

  componentDidMount() {
    const params = new URLSearchParams(window.location.search);

    const phoneNumber = params.get("phoneNumber");
    this.setState({
      verification: { ...this.state.verification, phoneNumber: phoneNumber },
    });
  }

  changeHandler(event) {
    const formData = { [event.target.name]: event.target.value };
    this.setState({
      verification: { ...this.state.verification, ...formData },
    });
  }

  submitHandler(event) {
    event.preventDefault();

    if (this.formIsValid()) {
      let code = parseInt(this.state.verification.verifyCode);
      this.setState({
        verification: { ...this.state.verification, verifyCode: code },
      });

      axios
        .post(API_ADDRESS + "Account/VerifyCode", {
          phoneNumber: this.state.verification.phoneNumber,
          verifyCode: this.state.verification.verifyCode,
        })
        .then((res) => {
          const url = window.location.href;
          const token = res.data.resul.token;
          localStorage.setItem("JWT", token);

          localStorage.setItem("userInfo", res?.data?.resul?.role);

          // if(res?.data?.resul?.role.toLowerCase() === "employee"){
          //   if(this.props.match.params.adverId){
          //     this.props.props.history.push = `/JobDetails/${this.props.match.params.adverId}`;
          //   }
          // }
          if (!(url.search("/Employee/") === -1)) {
            window.location.href = "/Employee/Jobs";
          } else if (!(url.search("/Employer/Login") === -1)) {
            window.location.href = "/Employer/Dashboard/";
            this.props.props.history.push("/Employer/Dashboard/");
          } else if (!(url.search("/Employer/Register") === -1)) {
            window.location.href = `/Employer/CompleteProfile?phoneNumber=${this.state.verification.phoneNumber}`;
          }
        })
        .catch((err) => {
          this.setState({ ...this.state, error: err.response.data.message[0] });
        });
    }
  }

  formIsValid() {
    let error = "";

    console.log(this.state.verification.verifyCode)
    let empty = validator.isEmpty(this.state.verification.verifyCode);

    if (empty) error = "???????? ???? ???????????????????? ???? ???????? ????????.";

    this.setState({ ...this.state, error: error });

    return !error;
  }

  resendCode = async () => {
    await axios.post(API_ADDRESS + "Account/Login", {
      phoneNumber: this.state.verification.phoneNumber,
    });

    await this.setState({
      verification: { ...this.state.verification, refresh: false },
    });

    await setTimeout(() => {
      this.setState({
        verification: {
          ...this.state.verification,
          refresh: true,
          timeToUpdate: 0,
          reset: true,
        },
      });
    }, 120000);
  };





  isInputValid(e) {
    let val = e.target.value;


    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '0');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '1');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '2');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '3');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '4');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '5');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '6');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '7');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '8');
    }
    if (val.match(/[??]/g)) {
      val = val.replace(/[??]/g, '9');
    }

    if (val.match(/[^0123456789]/g)) {
      val = val.replace(/[^0123456789]/g, '');
    }


    const verification = { ...this.state.verification };
    verification.verifyCode = val
    this.setState({ verification });

  };

  render() {
    if (auth.getCurrentUser()) {
      return <Redirect to="/" />;
    }

    setTimeout(() => {
      this.setState({
        verification: {
          ...this.state.verification,
          refresh: true,
          timer: 120000,
        },
      });
    }, 120000);

    return (
      <section className="container-fluid spx-2 spx-lg-10 smy-10 spt-10">
        <div className="row">
          <aside className="col-12 col-lg-5 mx-auto">
            <form className="w-100" onSubmit={this.submitHandler.bind(this)}>
              <div className="bg-white srounded-md sp-2 smb-2">
                <h1 className="fs-l c-dark d-block text-center smb-5 ir-bl">
                  ?????????? ???? ????????????????????
                </h1>

                <input placeholder="???? ??????????" type="text" className="w-100 m-0 my-2 py-2 px-3 text-right" value={this.state.verification.verifyCode} onChange={this.isInputValid.bind(this)}
                  style={{ border: "1px solid #9999", borderRadius: "10px", outline: "none" }} />



                <span className="d-block c-danger fs-s ir-r">
                  {this.state.error}
                </span>

                <div className="d-flex justify-content-between align-items-center smy-2 refresh-code ir-r">
                  {this.state.verification.refresh === true ? (
                    <button
                      id="refreshBtn"
                      onClick={this.resendCode}
                      className="btn px-0 shadow-none ir-r"
                    >
                      ?????????? ????????????
                    </button>
                  ) : (
                    <button
                      id="refreshBtn"
                      disabled
                      onClick={this.resendCode}
                      className="btn px-0 shadow-none ir-r"
                    >
                      ?????????? ????????????
                    </button>
                  )}

                  {this.state.verification.refresh === false ? (
                    <Timer
                      direction="backward"
                      initialTime={this.state.verification.timer}
                      reset={this.state.verification.reset}
                      checkpoints={[
                        {
                          time: 0,
                          callback: () =>
                            this.setState({
                              verification: {
                                ...this.state.verification,
                                refresh: true,
                              },
                            }),
                        },
                      ]}
                    >
                      <Timer.Minutes />
                      :
                      <Timer.Seconds />
                    </Timer>
                  ) : (
                    ""
                  )}
                </div>

                <button
                  type="submit"
                  className="btn btn-success ir-r d-block mx-auto"
                >
                  ??????????
                </button>
              </div>
            </form>
          </aside>
        </div>
      </section>
    );
  }
}
