import React, { Component } from "react";
import { Link, NavLink } from "react-router-dom";
import API_ADDRESS from "../../API_ADDRESS";
import ADDRESS from "../../ADDRESS";
import axios from "axios";
import KatinoServicesDropdown from "./components/katinoServicesDRD/katinoServicesDropdown";
import OffCanvasNavbar from './components/offCanvasNavbar/component';
import clsx from 'clsx';
import { toast } from "react-toastify";
import { history } from "../../core/agent";
import Cookies from 'js-cookie'

export class EmployerNavbar extends Component {


  state = {
    userInfo: {
      name: "",
      img: "",
      companyPersianName: "",
      companyEngName: ""
    },

    profileDropdown: false,
    profileActivity: "",

    notifDropdown: false,
    notifActivity: "",
    notifCount: 0,
    notifs: [],
    notifIds: [],
    currentPathname: "",
    unseenTicketsCount: 0
  };

  componentDidUpdate(prevProps, prevState) {
    if (prevState.currentPathname !== window.location.pathname) {
      this.setState({
        ...this.state,
        profileDropdown: false,
        profileActivity: "",
      });
      this.setState({ currentPathname: window.location.pathname });
    }
  }

  async componentDidMount() {

    this.setState({ currentPathname: window.location.pathname });

    await axios
      .get(API_ADDRESS + "Adver/GetAdverNotificationForUser", {
        headers: {
          Authorization: `bearer ${localStorage.getItem("JWT")}`,
        },
      })
      .then((res) => {
        let ids = [];

        res.data.resul.advernotifs.map((item) => ids.push(item.id));
        this.setState({
          notifCount: res.data.resul.notificationCount,
          notifs: res.data.resul.advernotifs,
          notifIds: ids,
        });
      });


    console.clear()
    axios
      .get(API_ADDRESS + "Account/GetEmployerInfo", {
        headers: { Authorization: `bearer ${localStorage.getItem("JWT")}` },
      })
      .then((res) => {
        console.log(res.data.resul)
        this.setState({
          userInfo: {
            companyPersianName: res.data.resul.companyPersianName,
            companyEngName: res.data.resul.companyEngName,
            name: res.data.resul.fullName,
            img: res.data.resul.image,
          },
        })

        if (res.data.resul.companyPersianName && res.data.resul.companyEngName) {
          Cookies.set('companyPersianName', res.data.resul.companyPersianName, { expires: 7 / 24 })
          Cookies.set('companyEngName', res.data.resul.companyEngName, { expires: 7 / 24 })
        }
      }

      );


    // alert(this.state.userInfo.companyPersianName)
    // alert(this.state.userInfo.companyEngName)
    // if (!this.state.userInfo.companyPersianName || !this.state.userInfo.companyEngName) {
    //   history.push('/Employer/CompleteProfile')
    //   return
    // }
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

      axios.post(
        API_ADDRESS + "Adver/SeenAdverNotification",
        this.state.notifIds,
        { headers: { Authorization: `bearer ${localStorage.getItem("JWT")}` } }
      );
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
      { id: 1, label: "میزکار", link: "/Employer/Dashboard" },
      { id: 2, label: "پروفایل شرکت", link: "/Employer/Profile" },
      { id: 3, label: "خرید اشتراک و امور مالی", link: "/Employer/Dashboard/Plans" },
      { id: 4, label: "شرکت های برتر", link: "/BestCompanies" },
      { id: 5, label: "اخبار", link: "/Blog?user=news" },
      { id: 6, label: "نمایندگان", link: "/Refrences" },
      {
        id: 7, label: "", link: "/",
        component: () => {
          return (<NavLink
            exact={true}
            activeClassName="activeLink"
            className=" text-nowrap btn btn-warning btn-sm ir-r text-white srounded-md text-decoration-none d-lg-flex justify-content-center align-items-center sml-2"
            to="/Employer/CreateAd"
          >
            درج آگهی جدید
          </NavLink>)
        }
      },
      {
        id: 8, label: "", link: "", hasInner: true,
        component: () => { return (<KatinoServicesDropdown color="#444" />) }
      }
    ];

    /*
    
    */


    return (
      <header className="g-header dash-nav bg-logo fixed-top d-flex justify-content-between align-items-center spx-2  navbar-shadow">

        <nav className="navbar navbar-expand-lg pr-0 py-0 flex-grow-1">
          <NavLink
            exact={true}
            className="d-none d-lg-inline text-white text-decoration-none navbar-brand p-0 m-0" to="/">
            خانه
          </NavLink>
          <OffCanvasNavbar menuIconColor="#fff" itemsList={employerNavlinks} />

          <div
            className="collapse navbar-collapse d-none d-lg-block"
            id="navbarSupportedContent"
          >

            <ul className="navbar-nav ml-auto">
              <li className="nav-item smr-lg-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-white position-relative ir-r fs-s p-0"
                  to="/Employer/Dashboard"
                >
                  میزکار
                </NavLink>
              </li>

              <li className="nav-item smr-lg-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-white positio -relative ir-r fs-s p-0"
                  to="/Employer/Profile"
                >
                  پروفایل شرکت
                </NavLink>
              </li>

              <li className="nav-item smr-lg-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-white position-relative ir-r fs-s p-0"
                  to="/Employer/Dashboard/Plans"
                >
                  خرید اشتراک و امور مالی
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-white text-center position-relative ir-r fs-s p-0"
                  to="/BestCompanies"
                >
                  شرکت های برتر
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  activeClassName="activeLink"
                  className="nav-link text-white text-center position-relative ir-r fs-s p-0"
                  to="/Blog?user=news"
                >
                  اخبار
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link  text-white text-center position-relative ir-r fs-s p-0"
                  to="/Refrences"
                >
                  نمایندگان
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <KatinoServicesDropdown />

              </li>

            </ul>
          </div>

          <div className="notification sml-2 mr-auto position-relative">
            <div className="head" onClick={this.notifDropdown}>
              <i className="far fa-bell icon text-white"></i>
              {this.state.notifCount === 0 ? (
                ""
              ) : (
                <span className="counter rounded-circle text-center ir-r bg-danger text-white position-absolute">
                  {this.state.notifCount}
                </span>
              )}
            </div>


            <div
              className={`notifs-dropdown bg-white position-absolute shadow ${this.state.notifActivity}`}
            >
              <ul className="m-0">
                {this.state.notifs.map((item, index) =>
                  index === 0 ? (
                    <li key={index} className=" text-right c-grey spy-1 fs-s">
                      <span className="ir-b">{`آگهی ${item.title}: `}</span>
                      <span className="ir-r">{item.adminDescription}</span>
                    </li>
                  ) : (
                    <li
                      key={index}
                      className="ir-r text-right c-grey spy-1 fs-s border-top"
                    >
                      {item.adminDescription}
                    </li>
                  )
                )}

                <li>
                  <NavLink
                    exact={true}
                    activeClassName="activeLink"
                    className="btn btn-primary-light ir-r d-block w-100"
                    to="/Employer/Notifications"
                  >
                    مشاهده ی اطلاعیه ها
                  </NavLink>
                </li>
              </ul>
            </div>

            {this.state.notifDropdown && <div onClick={this.notifDropdown} style={{
              zIndex: "900",
              position: "fixed",
              top: '0',
              right: "0",
              width: "100vw",
              height: "100vh",
              backgroundColor: "rgba(0,0,0,0)"
            }}
            ></div>}

          </div>

          <Link to="/Tickets" className="m-0 p-0 mx-2 position-relative text-decoration-none">
            <i style={{ fontSize: "1.3rem" }} class={clsx("fas fa-envelope-open-text icon ", this.state.unseenTicketsCount > 0 ? 'text-danger' : 'text-white')}></i>

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
        </nav>

        <div className="buttons d-flex justify-content-start align-items-center">
          <NavLink
            exact={true}
            activeClassName="activeLink"
            className=" text-nowrap btn btn-warning ir-r text-white srounded-md text-decoration-none d-none d-lg-flex justify-content-center align-items-center sml-2"
            to="/Employer/CreateAd"
          >
            درج آگهی جدید
            {/* <i className="fas fa-plus text-white"></i> */}
          </NavLink>


          <div
            id="MenuDropDown"
            className="user bg-white srounded-md sp-1 position-relative navbar navbar-expand-lg"
            onClick={this.profileDropdown}
          >
            <div className="head text-nowrap" id="MenuDropDown1">
              <span
                id="MenuDropDown2"
                className="ir-r fs-s sml-1 d-none d-inline"
              >
                {this.state.userInfo.name}
              </span>
              <i
                id="MenuDropDown3"
                className="fas fa-chevron-down text-white sp-1 bg-logo srounded-md"
              ></i>
            </div>

            <div
              className={`navBarContainer profile-dropdown bg-white position-absolute shadow ${this.state.profileActivity}  align-items-center`}
            >
              <ul className="m-0  align-items-center">
                <li className="responsive-menu smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none align-items-center"
                    to="/Employer/Dashboard"
                    onClick={() => {
                      this.setState({
                        ...this.state,
                        profileDropdown: false,
                        profileActivity: "",
                      });
                    }}
                  >
                    میزکار
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    exact={true}

                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/Profile"
                    onClick={() => {
                      this.setState({
                        ...this.state,
                        profileDropdown: false,
                        profileActivity: "",
                      });
                    }}
                  >
                    پروفایل شرکت
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/CreateAd"
                    onClick={() => {
                      this.setState({
                        ...this.state,
                        profileDropdown: false,
                        profileActivity: "",
                      });
                    }}
                  >
                    ایجاد آگهی استخدام
                  </NavLink>
                </li>

                <li className="smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/Dashboard"
                    onClick={() => {
                      this.setState({
                        ...this.state,
                        profileDropdown: false,
                        profileActivity: "",
                      });
                    }}
                  >
                    آگهی های استخدام
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/Dashboard/Plans"
                    onClick={() => {
                      this.setState({ profileDropdown: true });
                    }}
                  >
                    خرید اشتراک و امور مالی
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/EditProfile"
                    onClick={this.profileDropdown}
                  >
                    ویرایش اطلاعات شرکت
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/registersheba"
                    onClick={this.profileDropdown}
                  >
                    ثبت شماره شبا بانکی
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none"
                    to="/Employer/History/Payment"
                    onClick={this.profileDropdown}
                  >
                    تاریخچه حساب
                  </NavLink>
                </li>
                <li className="smb-1">
                  <NavLink
                    exact={true}
                    className="ir-r c-grey text-decoration-none"
                    to="/Tickets"
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
      </header >
    );
  }
}

