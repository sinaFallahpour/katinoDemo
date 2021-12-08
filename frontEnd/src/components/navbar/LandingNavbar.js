import React from "react";
import { Link, NavLink } from "react-router-dom";
import auth from "../../core/authService";
import ADDRESS from "../../ADDRESS";
import KatinoServicesDropdown from "./components/katinoServicesDRD/katinoServicesDropdown";
import OffCanvasNavbar from './components/offCanvasNavbar/component';

export class LandingNavbar extends React.Component {
  state = {
    profileDropdown: false,
    profileActivity: "",

    notifDropdown: false,
    notifActivity: "",
    notifCount: 10,
  };

  componentDidMount() {
    const user = auth.getCurrentUser();
    this.setState({ user });
  }

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

  render() {

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
      <header className="g-header dash-nav employee-nav fixed-top d-flex justify-content-between align-items-center spx-2  bg-white navbar-shadow">

        {/* Links */}
        <nav className="navbar navbar-expand-lg navbar-light pr-0 py-0">
          <NavLink
            exact={true}
            activeClassName="activeLink" className="navbar-brand p-0 m-0" to="/">
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
                  to="/"
                >
                  خانه
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="text-center nav-link position-relative ir-r fs-s p-0"
                  to="/Jobs?currentPage=1&pageSize=14"
                >
                  جستجوی مشاغل
                </NavLink>
              </li>
              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Employee/CreateResume"
                >
                  رزومه ساز
                </NavLink>
              </li>

              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/BestCompanies"
                >
                  شرکت های برتر
                </NavLink>
              </li>

              <li className="nav-item smr-lg-1 smr-xl-4">
                <NavLink
                  exact={true}
                  activeClassName="activeLink"
                  className="nav-link text-center position-relative ir-r fs-s p-0"
                  to="/Blog?user=news"
                >
                  اخبار
                </NavLink>
              </li>

              <li className="nav-item smr-lg-1 smr-xl-4">
                <KatinoServicesDropdown color="#00000080" />
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

            </ul>
          </div>
        </nav>

        {/* Buttons */}

        {!auth.getCurrentUser() ? (
          <div className="buttons d-flex justify-content-start">
            <NavLink
              exact={true}
              className="btn btn-warning ir-r d-none d-lg-block sml-1"
              to="/Employers"
            >
              <i className="fas fa-briefcase sml-1"></i>
              ثبت آگهی استخدام
            </NavLink>

            <NavLink
              exact={true}
              className="btn btn-primary ir-r" to="/Employee/Login/">
              <i className="fas fa-user sml-1"></i>
              ورود/ثبت نام
            </NavLink>
          </div>
        ) : (
          <div className="buttons d-flex justify-content-start align-items-center">
            <div
              className="user bg-primary srounded-md sp-1 position-relative"
              onClick={this.profileDropdown}
            >
              <div className="head">
                <span className="ir-r fs-s sml-1 text-white d-none d-lg-inline">
                  پروفایل
                </span>
                <i className="fas fa-chevron-down text-white srounded-md"></i>
              </div>

              <div
                className={`profile-dropdown bg-white position-absolute shadow ${this.state.profileActivity}`}
              >
                <ul className="m-0">
                  <li className="smb-1">
                    <NavLink
                      exact={true}
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/Dashboard/Requests"
                    >
                      درخواست های من
                    </NavLink>
                  </li>

                  <li className="smb-1">
                    <NavLink
                      exact={true}
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/plans"
                    >
                      خرید پلن
                    </NavLink>
                  </li>

                  <li className="smb-1">
                    <NavLink
                      exact={true}
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/Dashboard/Bookmarks"
                    >
                      آگهی های نشان شده
                    </NavLink>
                  </li>

                  <li className="smb-1">
                    <NavLink
                      exact={true}
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/Home"
                    >
                      ایمیل های اطلاع رسانی
                    </NavLink>
                  </li>

                  <li className="smb-1">
                    <NavLink
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/CreateResume"
                    >
                      رزومه ساز
                    </NavLink>
                  </li>

                  <li className="smb-1">
                    <NavLink
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/Home"
                    >
                      مشاهده رزومه
                    </NavLink>
                  </li>

                  <li className="smb-1">
                    <NavLink
                      activeClassName="activeLink"
                      className="ir-r c-grey text-decoration-none"
                      to="/Employee/Home"
                    >
                      تنظیمات حساب کاربری
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
        )}
      </header>
    );
  }
}
