import React, { Component } from "react";
import { Link } from "react-router-dom";
import {
  SearchBox,
  Companies,
  ResumeBuilder,
  Blog,
  Ads,
  companyService,
  adsServices,
} from "../components/home";

import { Markup } from 'interweave';

import { GetSharayetAkhzNamayande } from "../core/api/sharayetAkhzNamayande";

import ADDRESS from "../ADDRESS";



import { GetLandingPage } from "../core/api/landing-page";

import { citiesService } from "../components";
import * as service from "../components/blog";

import agent from "../core/agent";
import { toast } from "react-toastify";

export class Home extends Component {
  state = {

    companiesLogo: [],
    immediatelyAds: [],
    latestAds: [],
    cities: [],
    blog: [],
    LandingInfo: [],
    Landin_Resome_Title: "",
    Landin_Resome_Content: "",
    Landing_Banner: "",
    Landing_Img: "",
    sharayetAkhzNamayande: "",
    centeralOffice_address: "",
    centeralOffice_telephone: ""
  };

  async componentDidMount() {





    const data = await GetSharayetAkhzNamayande();
    this.setState({ sharayetAkhzNamayande: data.resul })

    // Cities for Search Box
    await citiesService
      .getCities()
      .then((res) => this.setState({ cities: res.data.resul }));

    // Companies Logo
    await companyService
      .getCompanies()
      .then((res) => this.setState({ companiesLogo: res.data.resul }));

    // Latest Ads
    await adsServices.getLatest().then((res) =>
      this.setState({
        latestAds: res.data.resul.listOfData.filter((_, indx) => indx < 16),
      })
    );

    // Immediately Ads
    await adsServices.getImmediately().then((res) =>
      this.setState({
        immediatelyAds: res.data.resul.listOfData.filter((_, indx) => indx < 8),
      })
    );

    await service.getBlogs().then((res) => this.setState({ blog: res.data.resul }));

    await GetLandingPage().then((res) =>
      res?.resul?.map((item) => {

        item.key === "Landin_Resome_Title" &&
          this.setState({ Landin_Resome_Title: item.value });

        item.key === "Landin_Resome_Content" &&
          this.setState({ Landin_Resome_Content: item.value });

        item.key === "Landing_Img" &&
          this.setState({ Landing_Img: item.value });

        item.key === "Landing_Banner" &&
          this.setState({ Landing_Banner: item.value });

        item.key === "MainCompanyAddress" &&
          localStorage.setItem("centeralOffice_address", item.value);

        item.key === "MainCompanyCompany" &&
          localStorage.setItem("centeralPffice_telephone", item.value);

        item.key === "gmail" &&
          localStorage.setItem("gmail", item.value);
      })
    );

    window.scrollTo(0, 0);
  }

  handleMarkOtherAdv = async (adverId, status) => {
    if (status === "latest") {
      try {
        let currentAdver = this.state.latestAds.find((c) => c.id == adverId);
        if (currentAdver.isMarked) {
          const newList = this.state.latestAds.map((el) =>
            el.id === adverId ? Object.assign({}, el, { isMarked: false }) : el
          );

          this.setState({
            latestAds: newList,
          });
          await agent.Adver.unmarkAdvder(adverId);
        } else {
          const newList = this.state.latestAds.map((el) =>
            el.id === adverId ? Object.assign({}, el, { isMarked: true }) : el
          );

          this.setState({
            latestAds: newList,
          });

          await agent.Adver.markAdvder(adverId);
        }
      } catch (ex) {
        this.setState({ isMarked: !this.state.isMarked });

        if (ex?.response?.data) {
          toast.error(ex.response?.data?.message[0]);
          const newList = this.state.latestAds.map((el) =>
            el.id === adverId
              ? Object.assign({}, el, { isMarked: !el.isMarked })
              : el
          );
          this.setState({
            latestAds: newList,
          });
        }
      }
    } else if (status === "immediate") {
      try {
        let currentAdver = this.state.immediatelyAds.find(
          (c) => c.id == adverId
        );
        if (currentAdver.isMarked) {
          console.log(this.state.immediatelyAds);
          const newList = this.state.immediatelyAds.map((el) =>
            el.id === adverId ? Object.assign({}, el, { isMarked: false }) : el
          );
          this.setState({
            immediatelyAds: newList,
          });
          await agent.Adver.unmarkAdvder(adverId);
        } else {
          console.log(this.state.immediatelyAds);
          const newList = this.state.immediatelyAds.map((el) =>
            el.id === adverId ? Object.assign({}, el, { isMarked: true }) : el
          );
          this.setState({
            immediatelyAds: newList,
          });

          await agent.Adver.markAdvder(adverId);
        }
      } catch (ex) {
        this.setState({ isMarked: !this.state.isMarked });

        if (ex?.response?.data) {
          toast.error(ex.response?.data?.message[0]);
          const newList = this.state.immediatelyAds.map((el) =>
            el.id === adverId
              ? Object.assign({}, el, { isMarked: !el.isMarked })
              : el
          );
          this.setState({
            immediatelyAds: newList,
          });
        }
      }
    }
  };

  handleMarkImediate = async (adverId) => { };

  render() {
    const { Landing_Banner } = this.state
    return (
      <div className="home">
        <SearchBox
          LandingImg={this.state.Landing_Img}
          props={this.props.props}
          cities={this.state.cities}
        />

        {(Landing_Banner && !Landing_Banner.includes('#')) &&
          <Link to="/BestCompanies" className="m-0 p-0">
            <section className="container-fluid spx-2 spx-lg-10 smy-10">
              <img className="w-100" src={`${ADDRESS}img/setting/${this.state.Landing_Banner}`} />
            </section>
          </Link>

        }

        <Companies logos={this.state.companiesLogo} />
        <Ads
          immediately={this.state.immediatelyAds}
          latest={this.state.latestAds}
          handleMarkOtherAdv={this.handleMarkOtherAdv}
        />

        <section className="container-fluid spx-2 spx-lg-10 smy-10 ir-bl">
          <div className="card mx-auto col-md-6 p-0">
            <div className="card-header bg-white border-0 p-0 m-0" >
              <div className="card-title bg-white p-0 m-0 w-100 h-100 d-flex flex-row justify-content-center align-items-center">
                <h3 className="text-center text-white p-3 m-0 w-75" style={{ background: "#273c85", fontSize:"1.4rem", borderRadius:"0 0 50% 50%" }}> شرایط اخذ نماینده </h3>
              </div>
            </div>
            <div className="card-body pt-0">
              <div className="text-center text-justify ir-r">
                {<Markup content={this.state.sharayetAkhzNamayande} />}
              </div>
            </div>
          </div>
        </section>


        {/* <section className="container-fluid spx-2 spx-lg-10 smy-10 ir-r">

          <div className="row">
            <div className="col-lg-3 col-sm-6 mb-4">
              <div className=" two"
                style={{
                  paddingTop: "40px !important",
                  borderRadius: "14px",
                  paddingBottom: "45px",
                  backgroundColor: "#19233c"
                }}>
                <figure className="text-center">
                  <img className="figure-img"
                    style={{
                      marginBottom: ".5rem",
                      lineHeight: "1",
                    }}

                    src="https://katino.ir//Uploads/Services/0ec98956-6799-432a-aef6-fca712a50b5e.jpg" />
                </figure>
                <h3
                  className="p-3 ir-bl"
                  style={{
                    color: "#ffffff",
                    fontSize: "20px",
                    marginBottom: "10px",
                  }}
                >     مشاوره و بازار یابی فروش  </h3>
                <p className="text-justify p-3"
                  style={{
                    color: "#ffffff",
                    marginBottom: "0",
                    fontWeight: "300",

                  }}
                >کاتینو با حمایت خود باعث میشود تا سخت کوشی و تلاش در کشور رواج پیدا کند و کسانی که میخواهند فعالیت جدید شغلی را&nbsp;شروع کنند در ابتدا و قدم اول حمایت شوند و از شکست&nbsp; واهمه نداشته باشند.</p>
              </div>
            </div>




            <div className="col-lg-3 col-sm-6 mb-4">
              <div className=" two"
                style={{
                  paddingTop: "40px !important",
                  borderRadius: "14px",
                  paddingBottom: "45px",
                  backgroundColor: "#19233c"
                  //      paddingBottom: 30px,
                  // background-color: #19233c,

                }}>
                <figure className="text-center">
                  <img className="figure-img"
                    style={{
                      marginBottom: ".5rem",
                      lineHeight: "1",
                    }}

                    src="https://katino.ir//Uploads/Services/0ec98956-6799-432a-aef6-fca712a50b5e.jpg" />
                </figure>
                <h3
                  className="p-3 ir-bl"
                  style={{
                    color: "#ffffff",
                    fontSize: "20px",
                    marginBottom: "10px",
                  }}
                >     خدمات ومشاوره حقوقی</h3>
                <p className="text-justify p-3"
                  style={{
                    color: "#ffffff",
                    marginBottom: "0",
                    fontWeight: "300",

                  }}
                >کاتینو با حمایت خود باعث میشود تا سخت کوشی و تلاش در کشور رواج پیدا کند و کسانی که میخواهند فعالیت جدید شغلی را&nbsp;شروع کنند در ابتدا و قدم اول حمایت شوند و از شکست&nbsp; واهمه نداشته باشند.</p>
              </div>
            </div>







            <div className="col-lg-3 col-sm-6 mb-4">
              <div className=" two"
                style={{
                  paddingTop: "40px !important",
                  borderRadius: "14px",
                  paddingBottom: "45px",
                  backgroundColor: "#19233c"
                  //      paddingBottom: 30px,
                  // background-color: #19233c,

                }}>
                <figure className="text-center">
                  <img className="figure-img"
                    style={{
                      marginBottom: ".5rem",
                      lineHeight: "1",
                    }}

                    src="https://katino.ir//Uploads/Services/0ec98956-6799-432a-aef6-fca712a50b5e.jpg" />
                </figure>
                <h3
                  className="p-3 ir-bl"
                  style={{
                    color: "#ffffff",
                    fontSize: "20px",
                    marginBottom: "10px",
                  }}
                >مشاوره انواع بیمه</h3>
                <p className="text-justify p-3"
                  style={{
                    color: "#ffffff",
                    marginBottom: "0",
                    fontWeight: "300",

                  }}
                >کاتینو با حمایت خود باعث میشود تا سخت کوشی و تلاش در کشور رواج پیدا کند و کسانی که میخواهند فعالیت جدید شغلی را&nbsp;شروع کنند در ابتدا و قدم اول حمایت شوند و از شکست&nbsp; واهمه نداشته باشند.</p>
              </div>
            </div>




            <div className="col-lg-3 col-sm-6 mb-4">
              <div className=" two"
                style={{
                  paddingTop: "40px !important",
                  borderRadius: "14px",
                  paddingBottom: "45px",
                  backgroundColor: "#19233c"
                  //      paddingBottom: 30px,
                  // background-color: #19233c,

                }}>
                <figure className="text-center">
                  <img className="figure-img"
                    style={{
                      marginBottom: ".5rem",
                      lineHeight: "1",
                    }}

                    src="https://katino.ir//Uploads/Services/0ec98956-6799-432a-aef6-fca712a50b5e.jpg" />
                </figure>
                <h3
                  className="p-3 ir-bl"
                  style={{
                    color: "#ffffff",
                    fontSize: "20px",
                    marginBottom: "10px",
                  }}
                >مشاوره انواع بیمه</h3>
                <p className="text-justify p-3"
                  style={{
                    color: "#ffffff",
                    marginBottom: "0",
                    fontWeight: "300",

                  }}
                >کاتینو با حمایت خود باعث میشود تا سخت کوشی و تلاش در کشور رواج پیدا کند و کسانی که میخواهند فعالیت جدید شغلی را&nbsp;شروع کنند در ابتدا و قدم اول حمایت شوند و از شکست&nbsp; واهمه نداشته باشند.</p>
              </div>
            </div>
          </div>
        </section> */}


        <ResumeBuilder
          title={this.state.Landin_Resome_Title}
          content={this.state.Landin_Resome_Content}
        />
        <Blog posts={this.state.blog} />
      </div>
    );
  }
}
