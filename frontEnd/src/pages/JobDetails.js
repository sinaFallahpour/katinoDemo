import React, { Component } from "react";
import {
  ShortDetails,
  Description,
  OtherAds,
  SendResume,
} from "../components/JobDetails";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import axios from "axios";
import API_ADDRESS from "../API_ADDRESS";
import { PageTitle } from "../components/PageTitle";
import agent from "../core/agent";
import {
  Accordion, Card, Button, Form
} from "react-bootstrap";
import StickyBox from "react-sticky-box";
import { swap } from "formik";

import { makeStyles } from '@material-ui/core/styles';
import InputLabel from '@material-ui/core/InputLabel';
import FormHelperText from '@material-ui/core/FormHelperText';
import './ad-style/style.css';

import Select from "react-select";
import { cooperationType, salary } from "../enums";

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  },
  selectEmpty: {
    marginTop: theme.spacing(2),
  },
}));

export class JobDetails extends Component {

  state = {
    loading: true,
    title2: "",
    description: "",
    name: "",
    phoneNumber: "",
    advertId: null,
    city: "",
    reportTitle: "دیگر"
  };

  gender = () => {
    switch (this.state.gender.toString()) {
      case "1":
        return "مهم نیست";

      case "2":
        return "مرد";

      case "3":
        return "زن";

      case "4":
        return "مهم نیست";

      default:
        return "مهم نیست";

    }
  }

  leastEdu = () => {
    switch (this.state.degreeOfEducation.toString()) {
      case "1":
        return "مهم نیست";

      case "2":
        return "دیپلم";

      case "3":
        return "کاردانی";

      case "4":
        return "کارشناسی";

      case "5":
        return "کارشناسی ارشد";

      case "6":
        return "دکترا";

      default:
        return "مهم نیست";

    }
  }

  leastResume = () => {
    switch (this.state.workExperience.toString()) {
      case "1":
        return "مهم نیست";

      case "2":
        return "کمتر از 3 سال";

      case "3":
        return "بین 3 تا 7 سال";

      case "4":
        return "بیشتر از 7 سال";

      default:
        return "مهم نیست";

    }
  }

  listenToPath = (e) => {
    alert(1)
    this.forceUpdate();
  };

  async componentDidMount() {
    const id = this.props.match.params.id;
    this.setState({ adverId: this.props.match.params.id });

    await axios
      .get(API_ADDRESS + `Adver/AdverDetails?id=${id}`)
      .then((res) => this.setState({ ...res.data.resul }));

    await axios
      .get(API_ADDRESS + "Adver/GetLastAdversForIndex?pageSize=6")
      .then((res) => this.setState({ latestAds: res.data.resul.listOfData }));

    this.setState({ loading: false });

    window.scrollTo(0, 0);
  }

  componentDidUpdate(prevProps, prevState) {

    if (prevProps.match.params.id !== this.props.match.params.id) {
      const id = this.props.match.params.id;
      this.setState({ adverId: this.props.match.params.id }, async () => {

        this.setState({ loading: true });
        await axios
          .get(API_ADDRESS + `Adver/AdverDetails?id=${id}`)
          .then((res) => this.setState({ ...res.data.resul }));


        this.setState({ loading: false });

        window.scrollTo(0, 0);
      });
    }
  }

  handleMark = async () => {
    try {
      if (this.state.isMarked) {
        this.setState({ isMarked: false });
        const { data } = await agent.Adver.unmarkAdvder(
          this.props.match.params.id
        );
      } else {
        this.setState({ isMarked: true });
        const { data } = await agent.Adver.markAdvder(
          this.props.match.params.id
        );
      }
    } catch (ex) {
      this.setState({ isMarked: !this.state.isMarked });
      if (ex?.response?.data) {
        toast.error(ex.response.data.message[0]);
      }
    }
  };

  handleMarkOtherAdv = async (adverId) => {
    try {
      let currentAdver = this.state.latestAds.find((c) => c.id == adverId);
      if (currentAdver.isMarked) {
        this.setState({
          latestAds: this.state.latestAds.map((el) =>
            el.id === adverId ? Object.assign({}, el, { isMarked: false }) : el
          ),
        });
        const { data } = await agent.Adver.unmarkAdvder(
          this.props.match.params.id
        );
      } else {
        this.setState({
          latestAds: this.state.latestAds.map((el) =>
            el.id === adverId ? Object.assign({}, el, { isMarked: true }) : el
          ),
        });

        const { data } = await agent.Adver.markAdvder(
          this.props.match.params.id
        );
      }
    } catch (ex) {
      this.setState({ isMarked: !this.state.isMarked });
      if (ex?.response?.data) {
        toast.error(ex.response?.data?.message[0]);
        this.setState({
          data: this.state.latestAds.map((el) =>
            el.id === adverId
              ? Object.assign({}, el, { isMarked: !el.isMarked })
              : el
          ),
        });
      }
    }
  };

  async changeHandler(event) {
    const formData = { [event.target.name]: event.target.value };
    await this.setState({ ...this.state, ...formData });
  }

  async submitHandler(event) {
    event.preventDefault();

    let obj = {
      title: this.state.reportTitle,
      description: this.state.description,
      name: this.state.name,
      phoneNumber: this.state.phoneNumber,
      advertId: this.props.match.params.id
      // const id = this.props.match.params.id;
    }
    try {
      var res = await agent.ReportAdvertS.create(obj);
      toast.success("با موفقیت انجام شد");
    }
    catch (err) {
      toast.success(err?.response?.data?.message[0]);
    }
  }

  returnLoading = (title) => {
    Swal.fire({
      title: title,
      allowEnterKey: false,
      allowEscapeKey: false,
      allowOutsideClick: false,
    });
    Swal.showLoading();
  };

  handleChange = async (e, { action, name }) => {
    this.setState({ reportTitle: e.label });
  };

  render() {

    const reports = [
      {
        value: 1,
        label: "آگهی غیر واقعی",
      },
      {
        value: 2,
        label: "استفاده ازالفاظ نامناسب ",
      },
      {
        value: 3,
        label: "صاحب آگهی در دسترس نیست ",
      },
      {
        value: 4,
        label: "حقوق نامناسب ",
      },
      {
        value: 5,
        label: "شماره تماس اشتباه",
      },
      {
        value: 6,
        label: "آدرس غیر واقعی ",
      },
      {
        value: 7,
        label: "گروه بندی نامناسب",
      },
      {
        value: 8,
        label: " آگهی نامناسب",
      },
      {
        value: 9,
        label: "دیگر",
      }
    ];

    /*
    
    اگهی غیر واقعی 
استفاده ازالفاظ نامناسب 
صاحب آگهی در دسترس نیست 
حقوق نامناسب 
شماره تماس اشتباه
آدرس غیر واقعی 
گروه بندی نامناسب 
دیگر

    */

    if (this.state.loading)
      return (

        <div className="ad-details container-fluid spx-2 spx-lg-10 smt-10 spt-5 smb-10">
          <div className="row">
            <div className="col-12 col-lg-12">
              <div className="bg-white srounded-md sp-2 smb-5 text-center ir-r">
                در حال بارگذاری...
              </div>
            </div>
          </div>
        </div>
      );
    else
      return (
        <PageTitle title={this.state.title}>
          <div className="ad-details container-fluid spx-2 spx-lg-10 smt-10 spt-5 smb-10">
            <div className="d-flex flex-row flex-wrap justify-content-center align-items-start m-0 p-0">
              <div className=" col-lg-8 col-md-9 col-12">

                {/* adver view details card */}
                <div style={{
                  border: '1px solid #273c85',
                  borderBottomWidth: "2px",
                  borderTop: '0',
                  boxShadow: "0 3px 6px 3px #ccc"
                }} class="m-0 bg-white srounded-md my-3 mt-0 p-0 d-flex flex-column justify-content-start align-items-stretch">

                  {/* adver header */}
                  <div className="m-0 p-0 d-flex flex-row justify-content-center align-items-stretch">
                    <div className="m-0 p-0 col-3"></div>
                    <div
                      style={{ height: "40px", borderRadius: "0 0 50% 50%", background: "#273c85" }}
                      className="m-0 p-0 text-white ir-b col-6 d-flex flex-row justify-content-center align-items-center">
                      {this.state.title}
                    </div>
                    <div className="m-0 p-0 pl-3 col-3 d-flex flex-row justify-content-end align-items-center">
                      <i
                        onClick={this.handleMark}
                        className={`bookmarker-btn c-dark fs-l ${this.state.isMarked === false ? "far" : "fas"
                          } fa-bookmark`}
                      ></i>
                    </div>
                  </div>

                  {/* adver featurs */}
                  <div className="ad3-body ir-r text-decoration-none mt-2 d-flex flex-row flex-wrap justify-content-between align-items-stretch">

                    <>

                      <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-between align-items-center">
                        <div className=" ad2-feature__key co  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fa fa-file-contract fa-2x"></i>
                          <span className="ad2-feature__key__name m-0 mr-1 p-1 ir-r">نوع قرارداد: </span>
                        </div>
                        <div className="ad2-feature__value  m-0   text-left">
                          {cooperationType(this.state.typeOfCooperation)}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-between align-items-center">
                        <div className=" ad2-feature__key   d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fa fa-tasks fa-2x"></i>
                          <span className="ad2-feature__key__name m-0 mr-1 p-1 ir-r">حوزه فعالیت: </span>
                        </div>
                        <div className="ad2-feature__value m-0  text-left ir-r">
                          {this.state && this.state.feildOfActivity}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-between align-items-center">
                        <div className=" ad2-feature__key c  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fas fa-money-check-alt fa-2x"></i>
                          <span className="ad2-feature__key__name m-0 mr-1 p-1">میزان حقوق: </span>
                        </div>
                        <div className="ad2-feature__value  m-0  text-left">
                          {salary(this.state.salary)}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-between align-items-center">
                        <div className=" ad2-feature__key   d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fas fa-school fa-2x"></i>
                          <span className="ad2-feature__key__name m-0 mr-1 p-1 ">حداقل تحصیلات: </span>
                        </div>
                        <div className="ad2-feature__value m-0  text-left">
                          {this.state && <>{this.leastEdu()}</>}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-between align-items-center">
                        <div className=" ad2-feature__key c  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fas fa-venus-mars fa-2x"></i>
                          <span className="ad2-feature__key__name m-0  p-1">جنسیت: </span>
                        </div>
                        <div className="ad2-feature__value  m-0  text-left">
                          {this.state && <>{this.gender()}</>}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-between align-items-baseline">
                        <div className=" ad2-feature__key   d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fas fa-file fa-2x"></i>
                          <span className="ad2-feature__key__name m-0 mr-1 p-1 ">حداقل سابقه کار: </span>
                        </div>
                        <div className="ad2-feature__value m-0  text-left">
                          {this.state && <>{this.leastResume()}</>}
                        </div>
                      </div>


                      <div className="ad2-feature col-lg-6 col-12 my-2   d-flex flex-row justify-content-between align-items-center">
                        <div className=" ad2-feature__key c  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fas fa-city fa-2x"></i>
                          <span className="ad2-feature__key__name m-0  p-1">شهر: </span>
                        </div>
                        <div className="ad2-feature__value  m-0  text-left">
                          {this.state && <>{this.state.city}</>}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-6 col-12 my-2    d-flex flex-row justify-content-between align-items-baseline">
                        <div className=" ad2-feature__key   d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fa fa-phone fa-2x"></i>
                          <span className="ad2-feature__key__name m-0 mr-1 p-1 ">شماره تماس  : </span>
                        </div>
                        <div className="ad2-feature__value m-0  text-left">
                          {this.state && <>{this.state.phoneNumber}</>}
                        </div>
                      </div>

                      <div className="ad2-feature col-lg-12 my-2   d-flex flex-row justify-content-start align-items-center">
                        <div className=" ad2-feature__key c  d-flex flex-row justify-content-start align-items-center p-1 m-0">
                          <i className="ad2-feature__key__icon fas fa-address-card fa-2x"></i>
                          <span className="ad2-feature__key__name m-0  p-1">آدرس محل کار: </span>
                        </div>
                        <div className="ad2-feature__value  m-0  text-right">
                          {this.state && <>{this.state.address}</>}
                        </div>
                      </div>

                    </>


                  </div>

                  {/* adver description */}

                  <Description isImmediate={this.state.isImmediate} adverId={this.props?.match?.params?.id} description={this.state.descriptionOfJob} />

                </div>

                {/* other advers */}
                <OtherAds
                  handleMarkOtherAdv={this.handleMarkOtherAdv}
                  list={this.state.latestAds}
                  feildOfActivity={this.state.feildOfActivity}
                  typeOfCooperation={this.state.typeOfCooperation}
                  jobTitle={this.state.title}
                  city={this.state.city}
                />
              </div>

              <div className="col-12 mt-3 col-lg-4">
                <SendResume id={this.state.id} />
                <div className="mt-4 send-resume bg-white srounded-md sp-2">
                  <Accordion defaultActiveKey="1">
                    <Card style={{overflow:'unset'}}>
                      <Card.Header className="w-100">
                        <Accordion.Toggle as={Button} variant="link" eventKey="0" className="w-100 btn btn-link btn btn-primary text-white ir-r">
                          ثبت گزارش تخلف
                        </Accordion.Toggle>
                      </Card.Header>
                      <Accordion.Collapse eventKey="0">
                        <Card.Body>

                          <Form
                            onSubmit={this.submitHandler.bind(this)}
                          >
                            <Form.Group controlId="formBasicEmail">
                              <Form.Label className="ir-r">  عنوان </Form.Label>
                              <Select
                                className="ir-r"
                                onChange={this.handleChange}
                                value={this.state.reportTitle}
                                options={reports}
                                isSearchable={true}
                                placeholder={this.state.reportTitle}
                                name="reportTitle"
                              // styles={this.state.customStyles}
                              />


                            </Form.Group>


                            <Form.Group controlId="formBasicEmail">
                              <Form.Label className="ir-r">  شماره تماس  </Form.Label>
                              <Form.Control required type="number " name="phoneNumber" className="ir-r" placeholder=""
                                value={this.state.phoneNumber || ""}
                                onChange={this.changeHandler.bind(this)}

                              />
                            </Form.Group>

                            <Form.Group controlId="formBasicEmail">
                              <Form.Label className="ir-r">   توضیحات  </Form.Label>
                              <Form.Control required type="text" className="ir-r" name="description" type="textarea" placeholder=""
                                value={this.state.description || ""}
                                onChange={this.changeHandler.bind(this)}

                              />
                            </Form.Group>

                            {/* 
                            <Form.Group controlId="formBasicPassword">
                              <Form.Label>Password</Form.Label>
                              <Form.Control type="password" placeholder="Password" />
                            </Form.Group>
                            
                            <Form.Group controlId="formBasicCheckbox">
                              <Form.Check type="checkbox" label="Check me out" />
                            </Form.Group>
                             */}

                            <Button variant="primary" type="submit">
                              ثبت
                            </Button>
                          </Form>

                        </Card.Body>
                      </Accordion.Collapse>
                    </Card>

                  </Accordion>
                </div>
              </div>


            </div>
          </div>
        </PageTitle>
      );
  }
}
