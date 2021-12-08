import React, { Component } from "react";
import { EmployerNavbar } from "./EmployerNavbar";
import { EmployeeNavbar } from "./EmployeeNavbar";
import { LandingNavbar } from "./LandingNavbar";
import { GetLandingPage } from "../../core/api/landing-page";
import KatinoServicesDropdown from './components/katinoServicesDRD/katinoServicesDropdown';

const commonNavLinks = [
  {id:0, label:"خانه", link:"/"},
  {id:1, label:" جستجوی مشاغل", link:"/Jobs"},
  {id:2, label:"رزومه ساز", link:"/Employee/CreateResume"},
  {id:3, label:"شرکت های برتر", link:"/BestCompanies"},
  {id:4, label:"اخبار", link:"/Blog?user=news"},
  {id:5, label:"", link:"",component:<KatinoServicesDropdown color="#00000080" />},
  {id:6, label:"نمایندگان", link:"/Refrences"},
  {id:7, label:"ثبت آگهی استخدام", link:"/Employers"},
  {id:8, label:"ورود/ثبت نام", link:"/Employee/Login/"},
];


export class Navbar extends Component {
  state = { userInfo: "", Logo: "" };

  async componentDidMount() {
    const role = localStorage.getItem("userInfo");
    await this.setState({ userInfo: role });

    GetLandingPage().then((res) =>
      res?.resul?.map((item) => {
        item.key === "Logo" && this.setState({ Logo: item.value });
      })
    );
  }

  render() {
    if (this.state.userInfo === "Employer") {
      return <EmployerNavbar Logo={this.state.Logo} />;
    } else if (this.state.userInfo === "Employee") {
      return <EmployeeNavbar Logo={this.state.Logo} />;
    } else {
      return <LandingNavbar Logo={this.state.Logo} />;
    }
  }
}
