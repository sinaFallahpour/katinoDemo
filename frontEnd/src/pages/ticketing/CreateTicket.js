import React, { Component } from "react";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import { citiesService } from "../../components/citiesService";
import Select from "react-select";
import agent from "../../core/agent";

export class CreateTicket extends Component {
  state = {
    details: {},
    id: null,
    fileName: "انتخاب و آپلود فایل ضمیمه",
    content: "",
    ticketPriorityStatus: 1,
    file: null,
    subject: null,
    cities: [],
    City: "",
    support_katinoJob_checked: true
  };

  componentDidMount() {
    citiesService.getCities().then((res) => {
      let cities = [];
      cities.push({
        value: "",
        label: `پشتیبانی کاتینو جاب`,
      });
      res.data.resul.map((item) => {
        cities.push({
          value: item.cityDivisionCode,
          label: ` ${item.provinceName}، ${item.cityName} `,
        });
      });
      this.setState({ cities });
    });
  }

  submitTicketAnswer = async (event) => {
    event.preventDefault();

    try {
      let datas = new FormData();

      datas.append("Subject", this.state.subject);
      datas.append("TicketPriorityStatus", this.state.ticketPriorityStatus);
      datas.append("Content", this.state.content);
      datas.append("File", this.state.file);
      datas.append("City", this.state.City);

      let { data } = await agent.Ticket.createTicket(datas);
      this.props.history.push("/Tickets");
      toast.success("ثبت موفقیت آمیز");
    } catch (err) {
      if (err.response.status === 401) toast.error("لطفا وارد شوید.");
      else if (err.response.status === 404) toast.error("خطای رخ داده  ");
      else if (err.response.status === 500) toast.error("مشکلی رخ داده ");
      else {
        for (let index = 0; index < err.response.data.message.length; index++) {
          toast.error(err.response.data.message[index]);
        }
      }
    }
  };

  fileHandler = (event) => {
    this.setState({ file: event.target.files[0] });
  };

  render() {
    return (
      <section className="ticket-detail container-fluid spx-2 spx-lg-10 smt-10 spt-3">
        <div className="row">
          <aside className="col-12 col-lg-6 mx-auto">
            <div className="sbs-shadow srounded-md bg-white sp-2">
              <header className="header d-flex flex-column flex-lg-row justify-content-between align-items-center">
                <h2 className="fs-m c-dark ir-b text-center text-lg-right w-50 text-truncate smb-2 mb-lg-0">
                  ثبت تیکت جدید
                </h2>

                <Link
                  className="ir-r fs-s btn bg-white shadow-none border"
                  to="/Tickets"
                >
                  بازگشت
                </Link>
              </header>

              <hr className="smy-2" />

              <span className="d-block text-right ir-b fs-s smb-1 c-regular">
                {this.state.details.receiverFullName}
              </span>

              <form onSubmit={this.submitTicketAnswer}>
                <input
                  className="form-control ir-r fs-s mt-0 smb-2 srounded-sm shadow-none sp-1"
                  style={{ resize: "none" }}
                  id="subject"
                  placeholder="موضوع پیام"
                  rows="4"
                  onChange={(e) => this.setState({ subject: e.target.value })}
                />

                <div className="choose-support m-0 my-2 ir-b p-0 d-flex flex-column justify-content-start align-items-stretch">
                  {/* <h3 style={{ fontSize: "1rem" }} classNamne="ir-b">انتخاب پشتیبانی</h3>
                  <div className="w-100 ir-r m-0 p-0 d-flex flex-row flex-wrap justify-content-start align-items-baseline">

                    <div className="m-0 p-0 pl-md-1 pl-0  w-100 d-flex flex-row justify-content-start align-items-baseline">

                      <label htmlFor="katino_job" className="m-0 mr-1 p-0">
                        <input onChange={(e) => {
                          if (e.target.checked) {
                            this.setState({ City: "" });
                            this.setState({ support_katinoJob_checked: true });
                          }
                        }} name="job_support" id="katino_job" type="radio" className="" defaultChecked />
                        <span className="m-0 mr-1 p-0"> کاتینو جاب </span>
                      </label>

                      <label htmlFor="city_agent" className="m-0 p-0 mr-3 d-flex flex-row justify-content-start align-items-baseline">
                        <input onChange={(e) => {
                          if (e.target.checked) {
                            this.setState({ support_katinoJob_checked: false });
                          }
                        }} name="job_support" id="city_agent" type="radio" className="mr-l"  />
                        <span className="m-0 mr-2 p-0"> نماینده شهر </span>
                      </label>
                    </div>


                  </div> */}
                  {/* <span style={{ fontSize: ".9rem", border: "1px solid transparent" }} classNamne="ir-b ">انتخاب شهر</span> */}
                  {/* <select
                      className="form-control ir-r fs-s mt-2 smb-2 srounded-sm shadow-none"
                      style={{ resize: "none" }}
                      id="subject"
                      placeholder="شهر"
                      rows="4"
                      onChange={(e) => { this.setState({ City: e.target.value }) }}
                    >
                      {!this.state?.cities?.length !== 0 &&
                        this.state?.cities?.map(({ value, label }) => (
                          <option value={value}>{label}</option>
                        ))}
                    </select> */}
                  {this.state.cities && (
                    <Select
                      onChange={(event) => this.setState({ City: event.value })}
                      placeholder="ارسال به"
                      styles={{ fontFamily: "iransans-regular" }}
                      options={this.state?.cities}
                      style={{ color: "#555" }}
                      className="mb-2"
                    />)
                  }

                </div>
                <select
                  hidden={this.state.support_katinoJob_checked}
                  className="form-control ir-r fs-s mt-0 smb-2 srounded-sm shadow-none"
                  style={{ resize: "none" }}
                  id="subject"
                  placeholder="اولویت"
                  rows="4"
                  onChange={(e) => this.setState({ City: e.target.value })}
                >
                  {!this.state?.cities?.length !== 0 &&
                    this.state?.cities?.map(({ value, label }) => (
                      <option value={value}>{label}</option>
                    ))}
                </select>
                <select
                  className="form-control ir-r fs-s mt-0 smb-2 srounded-sm shadow-none"
                  style={{ resize: "none" }}
                  id="subject"
                  placeholder="اولویت"
                  rows="4"
                  onChange={(e) =>
                    this.setState({ ticketPriorityStatus: e.target.value })
                  }
                ><option value="1">فوری</option>
                  <option value="2">جهت اطلاع</option>
                  <option value="3">عادی</option>
                </select>




                <textarea
                  className="form-control ir-r fs-s mt-0 smb-2 srounded-sm shadow-none sp-1"
                  style={{ resize: "none" }}
                  id="message"
                  placeholder="پیام شما..."
                  rows="4"
                  onChange={(e) => this.setState({ content: e.target.value })}
                ></textarea>

                <input
                  className="d-none"
                  id="file"
                  type="file"
                  accept="image/jpeg,image/gif,image/png,application/pdf,.doc,.docx"
                  onChange={this.fileHandler}
                />

                <div className="d-flex justify-content-start align-items-center smb-2">
                  <label
                    htmlFor="file"
                    className="ir-r btn btn-light fs-s shadow-none border sp-1"
                  >
                    {this.state.fileName}
                  </label>
                </div>

                <button type="submit" className="ir-r btn btn-primary">
                  ثبت
                </button>
              </form>
            </div>
          </aside>
        </div>
      </section>
    );
  }
}
