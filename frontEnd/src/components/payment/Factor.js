import axios from "axios";
import React, { Component } from "react";
import { Link } from "react-router-dom";
import API_ADDRESS, { API_URL } from "../../API_ADDRESS";

import { numberSeparator } from "../../common";

import { toast, ToastContainer } from "react-toastify";
import Swal from "sweetalert2";

export class Factor extends Component {
  state = {
    errMessage: "",
    id: 0,
    modalStatus: false,
    discount: "",
    giftCode: 0,
    giftDiscount: 0,
  };

  componentDidMount = () => {
    this.setState({ id: this.props.id });
  };

  modalHandler = {
    isOpen: () => this.setState({ modalStatus: true }),
    isClose: () => this.setState({ modalStatus: false }),
  };

  changeHandler = (event) => {
    const formData = { [event.target.name]: event.target.value };
    this.setState({ ...this.state, ...formData });
  };



  discountSubmiter = (event) => {

    event.preventDefault();
    axios
      .get(API_URL + `CheckGiftCart?giftCode=${this.state.giftCode}`)
      .then((res) => {

        if (res.data.statusCode === 400) {

          toast.error(res.data.message);
          const prevState = { ...this.state };
          prevState.modalStatus = false;
          this.setState(prevState);

          Swal.fire({
            title: "خطا",
            text: res.data.message,
            timer: 3000,
            icon: "error",
            background: "red",
            width: '300px',
            iconColor: "#fff",
            confirmButtonText: "تایید",
          });

        }
        else {
          const prevState = { ...this.state };
          prevState.giftDiscount = parseFloat(res.data.resul.giftDiscount) / 100;
          prevState.modalStatus = false;
          this.setState(prevState);
        }
      }
      ).catch(ex => {
        console.log(ex)
      });
  };

  paymentSubmiter = (event) => {
    event.preventDefault();
    axios
      .post(
        API_URL +
        `paymentPlan?planId=${this.props.id}${this.state.giftCode !== 0
          ? `&giftCode=${this.state.giftCode}`
          : ""
        }`,
        {},
        {
          headers: {
            Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
          },
        }
      )
      .then((res) => {
        var ss = res?.data?.resul?.descriptor?.url;
        var link =
          ss ? ss : res?.data?.resul.url;
        window.location.href = link;
      })
      .catch((err) => {
        console.log(err);
        if (err.response?.status === 401) toast.error("لطفا وارد شوید.");
        else if (err.response?.status === 404) toast.error("خطای رخ داده ");
        else if (err.response?.status === 500) toast.error("مشکلی رخ داده ");
        else {
          for (
            let index = 0;
            index < err?.response?.data?.message?.length;
            index++
          ) {
            toast.error(err.response.data.message[index]);
          }
        }

        // this.setState({ errMessage: err.response?.data?.message[0] })
      });
  };

  calc_finalPrice = () => {
    const offedPrice = ((this.props?.price) - ((this.props?.price * this.props?.discount) / 100)) * (1 - this.state.giftDiscount);
    const h = (offedPrice * 0.09) + offedPrice;
    return h;
  };

  calc_vat = () => {
    const offedPrice = ((this.props?.price) - ((this.props?.price * this.props?.discount) / 100)) * (1 - this.state.giftDiscount);
    const h = (offedPrice * 0.09);
    return h;
  };

  calc_totalDicount = () => {
    const planDiscountPrice = (this.props?.price * this.props?.discount) / 100;
    const planDiscountedPrice = this.props?.price - planDiscountPrice;
    const giftDiscountPrice = (planDiscountedPrice * this.state.giftDiscount);
    
    return (planDiscountPrice + giftDiscountPrice);
  };

  render() {
    return (
      <React.Fragment>
        <ToastContainer rtl={true} />
        {/* Factor */}
        <div className="payment-factor">
          <header className="f-header d-flex justify-content-between align-items-">
            <span className="fs-m ir-b c-dark">بررسی و پرداخت صورت حساب</span>
            <Link
              className="c-dark ir-r text-decoration-none"
              to="/Employer/Dashboard/Plans"
            >
              بازگشت
              <i className="fas fa-angle-left smr-1"></i>
            </Link>
          </header>

          <form onSubmit={this.paymentSubmiter} className="w-100">
            <div className="f-item d-lg-flex justify-content-lg-between align-items-lg-center sp-1 smt-2">
              <div className="f-field ir-r spy-1">مبلغ سفارش:</div>
              <div className="f-field ir-r spy-1" style={{ textDecoration: "line-through" }}>
                {
                  this.props?.isFree ?
                    <div className="f-field ir-r spy-1" >
                      رایگان
                    </div>
                    :
                    <>
                      {this.props?.price !== undefined
                        ? `${numberSeparator(
                          this.props?.price
                        )} تومان`
                        : "در حال بار گذاری..."}
                    </>
                }

              </div>
            </div>



            {this.props?.isFree ? <></> :

              <>
                <div className="f-item d-lg-flex justify-content-lg-between align-items-lg-center sp-1 smt-2">
                  <div className="f-field ir-r spy-1">مبلغ با تخفیف:</div>

                  <div className="f-field ir-r spy-1" >
                    {this.props?.discount !== undefined && this.props.price !== undefined
                      ? `${numberSeparator(
                        ((this.props?.price) - ((this.props?.price * this.props?.discount) / 100)) * (1 - this.state.giftDiscount)
                      )} تومان`
                      : "در حال بار گذاری..."}
                  </div>
                </div>


                <div className="f-item d-lg-flex justify-content-lg-between align-items-lg-center sp-1">
                  <div className="f-field ir-r spy-1">تخفیف:</div>
                  <div className="f-field ir-r spy-1">
                    <span className="c-danger">
                      {this.props?.price !== undefined
                        ? `${numberSeparator(
                          // this.props.price * this.state.giftDiscount
                          this.calc_totalDicount()
                        )} تومان`
                        : "در حال بار گذاری..."}

                      <button
                        onClick={this.modalHandler.isOpen}
                        type="button"
                        className="btn shadow-none p-0 c-primary smr-1 text-decoration-none"
                      >
                        {` کد تخفیف دارید؟ `}
                      </button>
                    </span>
                  </div>
                </div>

                <div className="f-item d-lg-flex justify-content-lg-between align-items-lg-center sp-1">
                  <div className="f-field ir-r spy-1">مالیات بر ارزش افزوده:</div>
                  <div className="f-field ir-r spy-1">

                    {this.props?.price !== undefined
                      ? `${numberSeparator(
                        this.calc_vat()
                      )} تومان `
                      : "در حال بار گذاری..."}
                  </div>
                </div>

              </>
            }



            <div className="f-item d-lg-flex justify-content-lg-between align-items-lg-center sp-1">
              <div className="f-field ir-r spy-1">مبلغ قابل پرداخت:</div>
              <div className="f-field ir-r spy-1 c-success">

                {this.props?.isFree ?
                  <div className="f-field ir-r spy-1" >
                    رایگان
                  </div> :

                  <>
                    {this.props?.price !== undefined
                      ? `${numberSeparator(

                        // Math.floor(
                        //   this.props?.price * (1 - this.state.giftDiscount) * 1.09
                        // )

                        Math.floor(
                          this.calc_finalPrice()
                        )
                      )} تومان `
                      : "در حال بار گذاری..."}</>
                }

              </div>
            </div>

            <button
              onSubmit={this.paymentSubmiter}
              type="submit"
              className="btn btn-success ir-r spx-3 d-flex mx-auto"
            >

              {this.props?.isFree ?

                <>              تایید نهایی

                </> :

                <>اتصال به درگاه پرداخت</>
              }


            </button>

            <span className="d-block text-center c-danger ir-r smt-3">
              {this.state.errMessage}
            </span>
          </form>
        </div>
        {/* Discount Modal */}
        <div
          className={`p-discount-modal d-flex justify-content-center align-items-center ${this.state.modalStatus === true ? "active" : ""
            }`}
        >
          <div className="overlay" onClick={this.modalHandler.isClose}></div>

          <div className="content bg-white sbs-content srounded-md spy-5 spx-2 d-flex flex-column justify-content-center">
            <form onSubmit={this.discountSubmiter} className="w-100">
              <div className="input-group smb-2">
                <span className="d-block w-100 text-center ir-r c-dark smb-1">
                  لطفا کد تخفیف خود را وارد کنید:
                </span>
                <input
                  onChange={this.changeHandler}
                  name="giftCode"
                  type="text"
                  className="form-control ir-r srounded-sm shadow-none"
                />
              </div>

              <div className="buttons d-flex justify-content-center align-items-center">
                <button
                  onClick={this.modalHandler.isClose}
                  type="button"
                  className="btn ir-r shadow-none p-0 sml-2"
                >
                  انصراف
                </button>

                <button
                  type="submit"
                  className="btn ir-r shadow-none btn-success"
                >
                  ثبت
                </button>
              </div>
            </form>
          </div>
        </div>
      </React.Fragment>
    );
  }
}
