import React, { Component } from "react";
import { PageTitle } from "../../components";
import {
  Company,
  getCompanyDetails,
  CompanyAds,
} from "../../components/companies";
import { CompanySideBar } from "./CompanySideBar";
import ReactSticky from "react-sticky-box";
import "./companyDetails.styles.css";
import ReactHtmlParser from "react-html-parser";

export class CompanyDetails extends Component {
  state = {};

  componentDidMount = async () => {
    const enName = this.props.match.params.enName;

    await getCompanyDetails(enName).then((res) =>
      this.setState({
        company: res.data.resul.company,
        activeAds: res.data.resul.activeAdver,
        deactiveAds: res.data.resul.deactiveAdver,
      })
    );
  };

  render() {
    if (this.state.company)
      return (
        <PageTitle
          title={`درباره شرکت ${this.state.company.companyPersianName}`}
        >
          <section className="container-fluids px-2 spx-lg-100 smt-100 spt-3 smt-10 spt-3">
            <div className="row">
              <div className="col-xl-9 col-12  mx-auto">
                <header className="company-details-header srounded-md"></header>
              </div>

              <div className="col-xl-8 col-11 mx-auto p-0 company-details smt-10 companies">




                <div className="mb-3">
                  <Company
                    name={this.state.company.companyPersianName}
                    enName={this.state.company.companyEngName}
                    city={this.state.company.city}
                    description={this.state.company.description}
                    filedOfActivity={this.state.company.filedOfActivity}
                    logo={this.state.company.image}
                    rate={this.state.company.rate}
                    hasLink={false}
                  />
                </div>
                <div className="mb-5">
                  <div className="row">
                    <div className="col-12">
                      {this.state.company.description}
                    </div>
                  </div>
                </div>


                <div className="sideBarContainer">
                  <ReactSticky
                    className="rightSideBar"
                    offsetTop={100}
                    offsetBottom={50}
                  >
                    <CompanySideBar
                      email={this.state.company.email}
                      website={this.state.company.url}
                      isActive={this.state.company.isActive}
                      mobile={this.state.company.mobile}
                      phoneNumber={this.state.company.phoneNumber}
                      managementFullName={this.state.company.managementFullName}
                      numberOfStaff={this.state.company.numberOfStaff}
                      gallery={this.state.company.gallery}
                    />
                  </ReactSticky>




                  <div className="LeftSideBar">
                    <div className="m-0 p-2 bg-white rounded ir-r ">
                    {this.state?.company?.desc && ReactHtmlParser(this.state?.company?.desc)}
                    </div>
                    <CompanyAds
                      activeAds={this.state.activeAds}
                      deactiveAds={this.state.deactiveAds}
                    />
                  </div>
                </div>
              </div>
            </div>
          </section>
        </PageTitle>
      );
    else
      return (
        <PageTitle title="در حال بارگذاری...">
          <section className="container-fluid spx-2 spx-lg-100 smt-100 spt-3 smt-10 spt-3">
            <div className="row">
              <span className="ir-r bg-white srounded-md sp-3 d-block mx-auto">
                در حال بارگذاری...
              </span>
            </div>
          </section>
        </PageTitle>
      );
  }
}
