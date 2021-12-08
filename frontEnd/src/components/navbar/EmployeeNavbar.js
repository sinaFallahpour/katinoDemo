import React, { Component } from "react";
import { Link, NavLink } from "react-router-dom";
import auth from "../../core/authService";
import ADDRESS from "../../ADDRESS";
import "./navbar.style.css";
import axios from "axios";
import API_ADDRESS from "../../API_ADDRESS";
import KatinoServicesDropdown from "./components/katinoServicesDRD/katinoServicesDropdown";
import OffCanvasNavbar from './components/offCanvasNavbar/component';
import clsx from 'clsx';
import { toast } from "react-toastify";

export class EmployeeNavbar extends Component {
  state = {
    islogedIn: false,
    userInfo: {
      name: "",
      img: ""
    },
    profileDropdown: false,
    profileActivity: "",

    notifDropdown: false,
    notifActivity: "",
    notifCount: 10,
    unseenTicketsCount: 0
  };

  async componentDidMount() {
    const user = auth.getCurrentUser();
    this.setState({ user });
    // console.log('user : ', user)

    await axios
      .get(API_ADDRESS + "Account/LoadEmployeePersonalInformation", {
        headers: { Authorization: `bearer ${localStorage.getItem("JWT")}` },
      })
      .then((res) =>
        this.setState({
          userInfo: {
            name: res.data.resul.fullName,
            img: res.data.resul.image,
          },
        })
      );
  }

  getUnseenTicketsList = async () => {
    const url = "https://panel.katinojob.ir/api/Tickets/GetUnSeenTicketCount";
    try {

      const resp = await axios.get(url);
      if (resp.data?.resul) {
        console.log(resp.data.resul);
        this.setState({ unseenTicketsCount: parseInt(resp.data.resul) });
      }
    } catch (ex) {
      if (ex.response) {
        toast.error(ex.response[0]);
      }
    }
  };

  profileDropdown = () => {
    if (this.state.profileDropdown === false) {
      this.setState({
        ...this.state,
        profileDropdown: true,
        notifDropdown: false,
        profileActivity: "active",
        notifActivity: "",
      });
    } else {
      this.setState({
        ...this.state,
        profileDropdown: false,
        profileActivity: "",
      });
    }
  };

  notifDropdown = () => {
    if (this.state.notifDropdown === false) {
      this.setState({
        ...this.state,
        notifDropdown: true,
        profileDropdown: false,
        notifActivity: "active",
        profileActivity: "",
        notifCount: 0,
      });
    } else {
      this.setState({
        ...this.state,
        notifDropdown: false,
        notifActivity: "",
      });
    }
  };

  logout = async () => {
    await window.localStorage.clear();
    window.location.href = "/";
  };

  goto = (event) => {
    const list = [
      "MenuDropDown",
      "MenuDropDown1",
      "MenuDropDown2",
      "MenuDropDown3",
    ];
    if (!list.includes(event.target.id)) {
      this.setState({
        ...this.state,
        profileDropdown: false,
        profileActivity: "",
      });
    }
  };

  render() {
    document.body.addEventListener("click", this.goto);

    const employerNavlinks = [
      { id: 0, label: "خانه", link: "/" },
      { id: 1, label: "جستجوی مشاغل", link: "/Employee/Jobs" },
      { id: 2, label: "رزومه ساز", link: "/Employee/CreateResume" },
      { id: 3, label: "شرکت های برتر", link: "/Employee/BestCompanies" },
      { id: 4, label: "اخبار", link: "/Blog?user=news" },
      { id: 5, label: "نمایندگان", link: "/Refrences" },

      {
        id: 8, label: "", link: "", hasInner: true,
        component: () => { return (<KatinoServicesDropdown color="#444" />) }
      }
    ];


    return (
      <header className="g-header dash-nav employee-nav bg-white fixed-top d-flex justify-content-between align-items-center spx-2 navbar-shadow">
        {/* Links */}
        <nav className="navbar navbar-expand-lg navbar-light pr-0 py-0">
          <NavLink
            exact={true}
            activeClassName="activeLink" className="navbar-brand p-0 m-0"
            to="/">
            <img
              src={
                this.props.Logo && `${ADDRESS}img/setting/${this.props.Logo}`
              }
              height="40"
              alt="کاتینو"
              loading="lazy"
            />
          </NavLink>
          <OffCanvasNavbar itemsList={employerNavlinks} />

          <div
            className="collapse navbar-collapse d-none d-lg-block"
            id="navbarSupportedContent"
          >
            <ul className="navbar-nav ml-auto">
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Employee/Home"
                >
                  خانه
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  activeClassName="activeLink"
                  className="text-center nav-link position-relative ir-r fs-s p-0"
                  to="/Employee/Jobs?currentPage=1&pageSize=14"
                >
                  جستجوی مشاغل
                </NavLink>
              </li>

              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Employee/CreateResume"
                >
                  رزومه ساز
                </NavLink>
              </li>

              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Employee/BestCompanies"
                >
                  شرکت های برتر
                </NavLink>
              </li>

              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Blog?user=news"
                >
                  اخبار
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Refrences"
                >
                  نمایندگان
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <KatinoServicesDropdown color="#00000080" />
              </li>

            </ul>
          </div>
        </nav>

        {!auth.getCurrentUser() ? (
          <div className="buttons d-flex justify-content-start">

            <NavLink

              className="text-white btn btn-warning ir-r d-none d-lg-block sml-1"
              to="/"
            >
              <i className="text-white fas fa-briefcase sml-1"></i>ثبت آگهی
              استخدام
            </NavLink>

            <NavLink
              className="btn btn-primary ir-r" to="/Employee/Login/">
              <i className="fas fa-user sml-1"></i>ورود/ثبت نام
            </NavLink>

            {/* <a
              className="btn btn-primary ir-r"
              href="/Employee/Login/"
              dideo-checked="true"
            >
              <i className="fas fa-user sml-1"></i>ورود/ثبت نام
            </a> */}
          </div>
        ) : (
          <>

            <Link to="/Employee/Tickets" className="m-0 p-0 mx-3 mr-auto position-relative text-decoration-none">
              <i style={{ fontSize: "1.3rem" }} class={clsx("fas fa-envelope-open-text icon ", this.state.unseenTicketsCount > 0 ? 'text-danger' : 'text-primary')}></i>

              {this.state.unseenTicketsCount > 0 &&
                <span
                  style={{
                    position: "absolute",
                    top: "-1rem",
                    right: "-1rem",
                    border: "2px solid #ffc107",
                    borderRadius: "50%",
                    width: "1.5rem ",
                    height: "1.5rem "
                  }}
                  className="m-0  text-center  badge badge-danger d-flex flex-row justify-content-center align-items-center">
                  {this.state.unseenTicketsCount}
                </span>}

            </Link>

            <div className="buttons d-flex justify-content-start align-items-center">
              <div
                id="MenuDropDown"
                className="user bg-primary srounded-md sp-1 position-relative"
                onClick={this.profileDropdown}
              >
                <div className="head" id="MenuDropDown1">
                  <span
                    id="MenuDropDown2"
                    className="ir-r fs-s sml-1 text-white d-none d-inline"
                  >
                    {this.state.userInfo.name && this.state.userInfo.name}
                  </span>
                  <i
                    id="MenuDropDown3"
                    className="fas fa-chevron-down text-white srounded-md"
                  ></i>
                </div>

                <div
                  className={`navBarContainer profile-dropdown bg-white position-absolute shadow ${this.state.profileActivity}`}
                >
                  <ul className="m-0">
                    <li className="responsive-menu smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Employee/Home"
                        onClick={this.profileDropdown}
                      >
                        خانه
                      </NavLink>
                    </li>
                    <li className="smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Employee/Dashboard/Requests"
                        onClick={this.profileDropdown}
                      >
                        درخواست های من
                      </NavLink>
                    </li>
                    {/* important dont remove the plan  */}
                    {/* <li className="smb-1">
                    <Link
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/plans"
                    >
                      خرید اشتراک
                    </Link>
                  </li> */}
                    <li className="smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Employee/Dashboard/Bookmarks"
                        onClick={this.profileDropdown}
                      >
                        آگهی های نشان شده
                      </NavLink>
                    </li>
                    <li className="smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Employee/CreateResume"
                        onClick={this.profileDropdown}
                      >
                        مشاهده رزومه
                      </NavLink>
                    </li>
                    <li className="responsive-menu smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Blog"
                        onClick={this.profileDropdown}
                      >
                        وبلاگ
                      </NavLink>
                    </li>
                    <li className="responsive-menu smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Employee/BestCompanies"
                        onClick={this.profileDropdown}
                      >
                        شرکت های برتر
                      </NavLink>
                    </li>
                    <li className="smb-1">
                      <NavLink
                        activeClassName="activeLink"
                        className="ir-r c-grey text-decoration-none"
                        to="/Employee/Tickets"
                        onClick={this.profileDropdown}
                      >
                        تیکت های پشتیبانی
                      </NavLink>
                    </li>
                    <li className="mb-0">
                      <button
                        type="button"
                        className="ir-r btn c-danger p-0 text-decoration-none shadow-none"
                        onClick={this.logout}
                      >
                        خروج از حساب
                      </button>
                    </li>
                  </ul>
                </div>
              </div>
            </div>

          </>
        )}
      </header>
    );
  }
}
