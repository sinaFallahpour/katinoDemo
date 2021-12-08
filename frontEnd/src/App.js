import React, { Component } from "react";
import { BrowserRouter, Switch, Route, Redirect   } from "react-router-dom";
import { BrowserHistoryContext   } from "./historyStore";
import { LandingFooter, EmployerFooter, PageTitle } from "./components";
import {
  NotFound,
  Home,
  Login,
  Verification,
  Register,
  Jobs,
  Payment,
  JobDetails,
  CreateResume,
  AllCompanies,
  BestCompanies,
  BestRefrences,
  EmployerLanding,
  CompanyDetails,
  Blog,
  Post,
} from "./pages";
import {
  CompleteProfile,
  CreateAd,
  Dashboard,
  AdInfo,
  RequestDetails,
  EditAdver,
} from "./pages/employer";
import { EditProfileEmployer } from "./pages/employer/EditProfileEmployer";
import { EmployerNotification } from "./components/employerPanel/notification/notification";
import { EmployerSheba } from "./pages/employer/EmployerSheba/EmployerSheba";
import { Navbar } from "./components";
import ScrollToTop from "./components/ScrollToTop"
import { Plans } from "./pages/employer/Plans";
import { EmployeePlans } from "./pages/employee/Plans";
import { ToastContainer } from "react-toastify";
import { SuccessPayment } from "./components/payment/SuccessPayment";
import { EmployeeDashboard } from "./pages/employee";
import { Security } from "./core/Security";
import {
  Tickets,
  CreateTicket,
  CreateTicketEmployee,
  TicketDetail,
  EmployeeTickets,
  EmployeeTicketDetail,
} from "./pages/ticketing";

import { EmployeeSuccessPage } from "./pages/employee/EmployeePayment/SuccessPayment";
import { EmployeeFailurePage } from "./pages/employee/EmployeePayment/FailurePayment";
import { EmployerSuccessPage } from "./pages/employer/EmployerPayment/SuccessPayment";
import { EmployerFailurePage } from "./pages/employer/EmployerPayment/FailurePayment";
import { EmployerProfile } from "./pages/employer/EmployerProfile";
import { EmployerHistoryPayment } from "./pages/employer/HistoryPayment";
import { AboutUsPage } from "./pages/KatinoInfoPages/AboutUs";
import { KhadamatMa } from "./pages/KatinoInfoPages/KhadamatMa";
import { SharayetAkhzNamayande } from "./pages/KatinoInfoPages/sharayetAkhzNamayande";
import { ContactPage } from "./pages/KatinoInfoPages/Contact";
import { PolicyPage } from "./pages/KatinoInfoPages/Policy";
import { EmployerTraining } from "./pages/KatinoInfoPages/EmployerTraining";
import { MyRequestDetails } from "./pages/employee/MyRequest/MyRequestDetails";
import { MyPlansDetails } from "./pages/employer/MyPlansDetails";
import { FrequentQ } from "./pages/KatinoInfoPages/FrequentQ";
import { OnlinePaymentGuide } from './pages/KatinoInfoPages/OnlinePaymentGuide/onlinePaymentGuide';
import { NewsSubscribtion } from './pages/employee/newsSubscribtion/component';
import agent from '../src/core/agent'
import axios from "axios";

import Cookies from 'js-cookie'
import { NotFoundPage } from "./components/notFoundPage/notFoundPage";
import { history } from "../src/core/agent";
import API_ADDRESS from "./API_ADDRESS";


class App extends Component {

  state = {
    init: 0,
    loading: true,
    userInfo: "",
  };

  async getUserInfo() {
    const userInfo = localStorage.getItem("userInfo");
    this.setState({ userInfo });
  }

  async componentDidMount() {
    const res = await agent.Account.UpdateLastSeen();
    await this.siteVisit()
    const userInfo = localStorage.getItem("userInfo");
    await this.setState({ userInfo, loading: false });



    // axios
    //   .get(API_ADDRESS + "Account/GetEmployerInfo", {
    //     headers: { Authorization: `bearer ${localStorage.getItem("JWT")}` },
    //   })
    //   .then((res) => {
    //     // console.log(res.data.resul)
    //     // this.setState({
    //     //   userInfo: {
    //     //     companyPersianName: res.data.resul.companyPersianName,
    //     //     companyEngName: res.data.resul.companyEngName,
    //     //     name: res.data.resul.fullName,
    //     //     img: res.data.resul.image,
    //     //   },


    //     if (res.data.resul.companyPersianName && res.data.resul.companyPersianName) {

    //       Cookies.set('companyPersianName', res.data.resul.companyPersianName, { expires: 7 / 24 })
    //       Cookies.set('companyEngName', res.data.resul.companyEngName, { expires: 7 / 24 })
    //     }

    //   })

    //     this.setState({init :1})

    // Cookies.set('companyPersianName', res.data.resul.companyPersianName, { expires: 7 / 24 })
    // Cookies.set('companyEngName', res.data.resul.companyEngName, { expires: 7 / 24 })

  }



  componentWillUnmount() {
    this.getUserInfo();
  }

  async siteVisit() {

    let isvisited = Cookies.get('isVisited') // => 'value'
    if (!isvisited) {
      Cookies.set('isVisited', 'yes', { expires: 1 })
      const res2 = await agent.Account.SiteVisit();
    }
    return;
    // Cookies.set('isVisited', 'yes', { expires: 7 })
  }

  isEmployer = () => {
    const { userInfo } = this.state;
    if (!userInfo) return false;
    if (userInfo != "Employer") return false;
    return true;
  };


  isCommpleteProfile = () => {
    let companyPersianName = Cookies.get('companyPersianName')
    let companyEngName = Cookies.get('companyEngName')
    if (!companyPersianName || !companyEngName) {
      return false
    }
     return true

  };

  isEmployee = () => {
    const { userInfo } = this.state;
    if (!userInfo) return false;
    if (userInfo != "Employee") return false;
    return true;
  };

  isLogedIn = () => {
    const { userInfo } = this.state;
    if (!userInfo) return false;
    return true;
  };

  render() {
    if (this.state.loading) {
      return <>loading..</>;
    }

    return (
      <div className="App">
        <BrowserRouter history={history}>
          <BrowserHistoryContext>
            <Switch>
              <Route
                path="/employer/payment/success"
                exact
                render={(props) => {
                  return (
                    <PageTitle title="وضعیت پرداخت">
                      <EmployerSuccessPage />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>
              {/*employer failure payment  */}
              <Route
                path="/employer/payment/failure"
                exact
                render={(props) => {
                  return (
                    <PageTitle title="وضعیت پرداخت">
                      <EmployerFailurePage />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              {/*employee success payment  */}
              <Route
                path="/employee/payment/success"
                exact
                render={(props) => {
                  return (
                    <PageTitle title="وضعیت پرداخت">
                      <EmployeeSuccessPage />
                    </PageTitle>
                  );
                }}
              ></Route>
              {/*employee failure payment  */}
              <Route
                path="/employee/payment/failure"
                exact
                render={(props) => {
                  return (
                    <PageTitle title="وضعیت پرداخت">
                      <EmployeeFailurePage />
                    </PageTitle>
                  );
                }}
              ></Route>

              {/* <Navbar /> */}
              <Route
                path="/"
                exact={true}
                render={(props) => {
                  // //document.getElementById("root").scrollIntoView();
                  // document.getElementsByTagName("body")[0].scrollIntoView();
                  // window.scrollTo(0, -50);

                  return (
                    <PageTitle title="خانه">
                      <Navbar />
                      <Home {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employee/Home"
                exact
                render={(props) => {
                  // //document.getElementById("root").scrollIntoView();
                  // window.scrollTo(0, -50);
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="خانه">
                      <Navbar />
                      <Home {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Jobs"
                render={(props) => {
                  window.scrollTo(0, -50);
                  return (
                    <PageTitle title="جستجوی مشاغل">
                      <Navbar />
                      <Jobs {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employee/Jobs"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  window.scrollTo(0, 0);
                  return (
                    <PageTitle title="جستجوی مشاغل">
                      <Navbar />
                      <Jobs {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/:role/Login"
                render={(props) => (
                  <PageTitle title="ورود">
                    <Navbar />
                    <Login {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/:role/Register"
                render={(props) => (
                  <PageTitle title="ثبت نام">
                    <Navbar />
                    <Register {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                path="/:role/:type/Verification"
                render={(props) => (
                  <PageTitle title="کد اعتبارسنجی">
                    <Navbar />
                    <Verification props={props} {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                path="/Employer/CompleteProfile"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  window.scrollTo(0, -50);
                  return (
                    <PageTitle title="تکمیل پروفایل">
                      {/* <Navbar /> */}
                      <CompleteProfile props={props} {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employer/EditProfile"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  // if (!this.isCommpleteProfile())
                  //   return <Redirect exact to="/Employer/CompleteProfile" />
                  window.scrollTo(0, -50);
                  return (
                    <PageTitle title="ویرایش پروفایل">
                      <Navbar />
                      <EditProfileEmployer props={props} {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employer/Notifications"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="مشاهده اعلان ها">
                      <Navbar />
                      <EmployerNotification props={props} {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employer/Dashboard/Plans/:id/Payment"
                render={(props) => {
                  //document.getElementById("root").scrollIntoView();
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  else
                    return (
                      <PageTitle title="پرداخت">
                        <Navbar />
                        <Payment props={props} {...props} />
                        <EmployerFooter className="d-none d-lg-block" />
                        <LandingFooter />
                      </PageTitle>
                    );
                }}
              ></Route>

              <Route
                path="/Employer/Dashboard"
                exact
                render={(props) => {
                  if (!this.isEmployer()) {
                    return <Redirect exact to="/Employer/Login" />;
                  }
                  // if (!this.isCommpleteProfile() )
                  //   return <Redirect exact to="/Employer/CompleteProfile" />

                  return (
                    <PageTitle title="میزکار">
                      <Navbar />
                      <Dashboard {...props} />
                      <EmployerFooter className="d-none d-lg-block" />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employer/Dashboard/Plans"
                exact={true}
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="تعرفه ها">
                      <Navbar />
                      <Plans {...props} />
                      <EmployerFooter className="d-none d-lg-block" />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employer/CreateAd"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="درج آگهی">
                      <Navbar />
                      <CreateAd props={props} {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employer/editAdver"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="ویرایش آگهی">
                      <Navbar />
                      <EditAdver props={props} {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employee/newsSubscribtion"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="دریافت فرصت های شغلی جدید">
                      <Navbar />
                      <br />
                      <br />
                      <br />
                      <br />
                      <NewsSubscribtion props={props} {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employer/History/Payment"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="تاریخچه حساب">
                      <Navbar />
                      <EmployerHistoryPayment props={props} {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact={true}
                path="/Employer/MyPlansDetails"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="تاریخچه حساب">
                      <Navbar />
                      <MyPlansDetails props={props} {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employer/registersheba"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="ثبت شماره شبا">
                      <Navbar />
                      <EmployerSheba props={props} {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employer/SuccessPayment"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <PageTitle title="پرداخت با موفقیت انجام شد">
                      <Navbar />
                      <SuccessPayment {...props} />
                      <EmployerFooter />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/JobDetails/:id"
                render={(props) => {
                  console.log(props);
                  if (!this.isLogedIn() || this.isEmployer()) {
                    return <Redirect exact to={"/Employer/Login"} />;
                  }
                  return (
                    <PageTitle title="مشاهده آگهی">
                      <Navbar />
                      <JobDetails {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              {/* <Route
                path="/Employee/:adverId"
                render={(props) => {
                  console.log(props);
                  if (!this.isLogedIn() || this.isEmployer()) {

                    return <Redirect exact to={"/Employee/Login"} />;
                  }
                  return <Redirect exact to={"/JobDetails?adverId=" + props.match.params.adverId} />;

                }}
              ></Route> */}

              <Route
                path="/Employee/Plans"
                exact
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="تعرفه ها">
                      <Navbar />
                      <EmployeePlans {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employee/CreateResume"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="ساخت رزومه">
                      <Navbar />
                      <CreateResume {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/BestCompanies"
                render={(props) => (
                  <PageTitle title="50 شرکت برتر">
                    <Navbar />
                    <BestCompanies />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/Refrences"
                render={(props) => (
                  <PageTitle title="نمایندگان">
                    <Navbar />
                    <BestRefrences />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/Employee/BestCompanies"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="50 شرکت برتر">
                      <Navbar />
                      <BestCompanies />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/AllCompanies"
                render={(props) => (
                  <PageTitle title="همه ی شرکت ها">
                    <Navbar />
                    <AllCompanies />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/Employers"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  return (
                    <PageTitle title="بخش کارفرمایان">
                      <Navbar />
                      <EmployerLanding />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/Company/:enName"
                render={(props) => (
                  <React.Fragment>
                    <Navbar />
                    <CompanyDetails props={props} {...props} />
                    <LandingFooter />
                  </React.Fragment>
                )}
              ></Route>

              <Route
                exact
                path="/Employee/Dashboard/:page"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <React.Fragment>
                      <Navbar />
                      <EmployeeDashboard props={props} {...props} />
                      <LandingFooter />
                    </React.Fragment>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/Employee/Dashboard/Requests/:id"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="جزئیات درخواست من">
                      <Navbar />
                      <MyRequestDetails props={props} {...props} />
                      <LandingFooter />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/Employer/AdInfo/:id"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <React.Fragment>
                      <Navbar />
                      <AdInfo props={props} {...props} />
                      <EmployerFooter className="d-none d-lg-block" />
                      <LandingFooter />
                    </React.Fragment>
                  );
                }}
              ></Route>

              <Route
                path="/Employer/AdInfo/:id/RequestDetails/:resumeId/:asignResomeId"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employer/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />
                  return (
                    <React.Fragment>
                      <Navbar />
                      <RequestDetails props={props} {...props} />
                      <EmployerFooter className="d-none d-lg-block" />
                      <LandingFooter />
                    </React.Fragment>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/Blog"
                render={(props) => {
                  //document.getElementById("root").scrollIntoView();
                  window.scrollTo(0, 0);
                  return (
                    <PageTitle title="وبلاگ">
                      <Navbar />
                      <Blog props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  )
                }}
              ></Route>

              <Route
                exact
                path="/EmployerTraining"
                render={(props) => (
                  <PageTitle title="وبلاگ">
                    <Navbar />
                    <EmployerTraining props={props} {...props} />
                    <LandingFooter className="d-none d-lg-block" />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/FrequentQuestion"
                render={(props) => (
                  <PageTitle title="سوالات متداول">
                    <Navbar />
                    <FrequentQ props={props} {...props} />
                    <LandingFooter className="d-none d-lg-block" />
                  </PageTitle>
                )}
              ></Route>

              <Route
                path="/Blog/Post/:id"
                render={(props) => (
                  <React.Fragment>
                    <Navbar />
                    <Post props={props} {...props} />
                    <LandingFooter className="d-none d-lg-block" />
                  </React.Fragment>
                )}
              ></Route>

              <Route
                exact
                path="/Tickets"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="تیکت های پشتیبانی">
                      <Navbar />
                      <Tickets props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Tickets/:id"
                render={(props) => {
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="تیکت های پشتیبانی">
                      <Navbar />
                      <TicketDetail props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/Employee/Tickets"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="تیکت های پشتیبانی">
                      <Navbar />
                      <EmployeeTickets props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employee/Tickets/:id"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="تیکت های پشتیبانی">
                      <Navbar />
                      <EmployeeTicketDetail props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employee/createTicket"
                render={(props) => {
                  if (!this.isEmployee())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="ایجاد تیکت">
                      <Navbar />
                      <CreateTicketEmployee props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/Employer/Profile"
                render={(props) => {
                  //document.getElementById("root").scrollIntoView();
                  if (!this.isEmployer())
                    return <Redirect exact to="/Employee/Login" />;
                  if (!this.isCommpleteProfile())
                    return <Redirect exact to="/Employer/EditProfile" />

                  return (
                    <PageTitle title="پروفایل کارفرما">
                      <Navbar />
                      <EmployerProfile props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                path="/createTicket"
                render={(props) => {
                  //document.getElementById("root").scrollIntoView();
                  if (!this.isLogedIn())
                    return <Redirect exact to="/Employee/Login" />;
                  return (
                    <PageTitle title="ایجاد تیکت">
                      <Navbar />
                      <CreateTicket props={props} {...props} />
                      <LandingFooter className="d-none d-lg-block" />
                    </PageTitle>
                  );
                }}
              ></Route>

              <Route
                exact
                path="/Policy"
                render={(props) => (
                  <PageTitle title="قوانین کاتینو">
                    <Navbar />
                    <PolicyPage props={props} {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/onlinePaymentGuide"
                render={(props) => (
                  <PageTitle title="راهنمای پرداخت آنلاین">
                    <Navbar />
                    <OnlinePaymentGuide props={props} {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/Contact"
                render={(props) => {
                  document.getElementById("root").scrollIntoView(true);
                  return (
                    <PageTitle title="تماس با کاتینو">
                      <Navbar />
                      <ContactPage props={props} {...props} />
                      <LandingFooter />
                    </PageTitle>
                  )
                }}
              ></Route>

              <Route
                exact
                path="/AboutUS"
                render={(props) => {
                  //document.getElementById("root").scrollIntoView();
                  return (
                    <PageTitle title="درباره کاتینو">
                      <Navbar />
                      <AboutUsPage props={props} {...props} />
                      <LandingFooter />
                    </PageTitle>
                  )
                }}
              ></Route>

              <Route
                exact
                path="/KhadamatMa"
                render={(props) => (
                  <PageTitle title="خدمات ما">
                    <Navbar />
                    <KhadamatMa props={props} {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route
                exact
                path="/SharayetAkhzNamayande"
                render={(props) => (
                  <PageTitle title="شرایط اخذ نماینده">
                    <Navbar />
                    <SharayetAkhzNamayande props={props} {...props} />
                    <LandingFooter />
                  </PageTitle>
                )}
              ></Route>

              <Route path="/Security/:username/:key/:role" render={(props) =>
                <Security props={props} {...props} />
              } />

              {/* 
              <Route
                path="/notfound"
                render={(props) => {
                  return (
                    <PageTitle title="">
                      <NotFoundPage props={props} />
                    </PageTitle>
                  )
                }}
              ></Route> */}

              {/* <Route exact component={NotFoundPage} /> */}
              {/* <Redirect exact to="/notfound" /> */}
              {/* </ScrollToTop> */}
            </Switch>

          </BrowserHistoryContext>
          {/* <Footer/> */}
        </BrowserRouter>

        <div className="ir-r">
          <ToastContainer style={{ fontSize: "1rem" }} rtl={true} />
        </div>
      </div>
    );
  }
}

export default App;
