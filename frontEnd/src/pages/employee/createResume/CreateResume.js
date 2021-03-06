import React, { Component } from "react";
import axios from "axios";
import API_ADDRESS from "../../../API_ADDRESS";
import Select from "react-select";

import { toast } from "react-toastify";
import { SideBar } from "../../../components";
import agent, { avatarUrl } from "../../../core/agent";
import { salaries, typeOfCooperation, expriences } from "./salaries";
import { JobExpreinceFormGenerator } from "./JobExpreince/JobExpreinceFormGenerator";
import { JobExpreinceDetails } from "./JobExpreince/JobExpreinceDetails";
import { EducationalBackgroundFormGenerator } from "./EducationBackground/EducationBackgroundGenerator";
import { EducationalBackgroundDetails } from "./EducationBackground/EducationBackgroundDetails";
import { LanguageGenerator } from "./LanguageSection/LanguageGenerator";
import { LanguageDetails } from "./LanguageSection/LanguageDetails";
import { JobPreferenceDetails } from "./JobPreference/JobPreferenceDetails";
import Swal from "sweetalert2";
import { DatePickerModern } from "../../../core/utils/datepicker.util";
import "react-modern-calendar-datepicker/lib/DatePicker.css";
import { UploadPdf } from "./UploadPdf/UploadPdf";
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { makeStyles } from '@material-ui/core/styles';
import JobPreferences from './JobPreferences/component';


const useStyles = makeStyles({
  option: {
    fontSize: 15,
    '& > span': {
      marginRight: 10,
      fontSize: 18,
    },
  },
});

export class CreateResume extends Component {
  state = {
    hasImage: false,
    cities: [],
    editMode: false,
  };

  async componentDidMount() {
    document.getElementById("root").scrollIntoView();


    await axios
      .get(`${API_ADDRESS}UserJobShortDescription/GetUserShortDescription`, {
        headers: {
          Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
          "content-type": "application/json;charset=utf-8",
        },
      })
      .then((res) => this.setState({ info: res.data.resul }));

    const res = await agent.Cities.Cities();

    const { data } = await agent.CreateResome.loadEmployeePersonalInformation();
    const resAboutMe = await agent.CreateResome.LoadEmployeeAboutMe();
    const resJobSkill = await agent.CreateResome.GetAlljobSkillsForSelect();
    const resuserJobSkill = await agent.CreateResome.getAllUserJobSkillsForCurrentUser();
    const AllCategories = await agent.CreateResome.getAllCategories();
    const resuJobPreference = await agent.CreateResome.GetUserJobPreferenceForCurrentUser();
    const getAllWorkExperience = await agent.CreateResome.GetAllWorkExperience();
    const getAllEduBackground = await agent.CreateResome.GetAllEduBackground();
    console.log('getAllEduBackground : ', getAllEduBackground)
    const getAllLanguageForCurrentUser = await agent.CreateResome.GetAllLanguageForCurrentUser();

    const resuJobPreferenceIds =
      (await resuJobPreference) &&
      resuJobPreference?.data?.resul?.categoryForJobPrefence &&
      resuJobPreference?.data?.resul?.categoryForJobPrefence?.map(
        ({ categoryId }) => categoryId
      );

    let Category = await AllCategories?.data?.resul?.map(({ id, name }) => {
      return { value: id, label: name };
    });
    await this.getResomePercent();

    await this.setState({
      info2: data.resul,
      cities: res.data.resul,
      categories: Category,
      jobSkills: resJobSkill.data.resul,
      userJobSkills: resuserJobSkill.data.resul,
      aboutMen: resAboutMe.data.resul,
      getAllWorkExperience: getAllWorkExperience.data.resul,
      getAllEduBackground: getAllEduBackground.data.resul,
      getAllLanguageForCurrentUser: getAllLanguageForCurrentUser.data.resul,
      jobPreferenceStatus: resuJobPreference.data.resul,
      resuJobPreferenceIds: resuJobPreferenceIds,
      info8: resuJobPreference.data.resul || {
        city: "",
        typeOfCooperation: 0,
        senioritylevel: 0,
        salary: 0,
        promotion: 0,
        insurance: 0,
        educationCourses: 0,
        flexibleWorkingTime: 0,
        hasMeel: 0,
        transportationService: 0,
        categoryIds: [],
      },
    });
  }

  getResomePercent = async () => {
    // event.preventDefault();

    try {
      let { data } = await agent.CreateResome.GetResomePercent();
      this.setState({ resomePercent: data.resul });
    } catch (err) {
      if (err.response.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response.status === 404) toast.error("???????? ???? ????????  ");
      else if (err.response.status === 500) toast.error("?????????? ???? ???????? ");
      else toast.error(err.response.message[0]);
    }
  };

  async avatarHandler(event) {
    await this.setState({
      avatarSRC: URL.createObjectURL(event.target.files[0]),
    });

    let formData = new FormData();

    formData.append("image", document.getElementById("avatar").files[0]);

    axios
      .post(API_ADDRESS + "Account/EmployeeChangeAvatar", formData, {
        headers: {
          Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
        },
      })
      .then(async (data) => {
        await this.setState({
          info2: { ...this.state.info2, employeeAvatar: data.data.resul },
        });
      })
      .catch(
        this.setState({
          errors: {
            avatar: "?????????? ???? ???????? ?????? ???????? ???????????? ???????????? ????????.",
          },
        })
      );
  }

  editDesc() {
    this.setState({ editMode: true });
  }

  cancel() {
    this.setState({ editMode: false });
    this.setState({ editMode2: false });
    this.setState({ editMode3: false });
    this.setState({ editMode4: false });
  }

  async radionHandler(event) {
    await this.setState({
      info: {
        ...this.state.info,
        employmentStatus: parseInt(event.target.value),
      },
    });
  }

  returnEmploymentStatus = () => {
    var employmentStatus = this.state.info?.employmentStatus;
    if (!employmentStatus) return;
    if (employmentStatus == 1) return "?????????? ??????";
    if (employmentStatus == 2) return "????????";
    if (employmentStatus == 3) return "???? ?????????? ?????? ????????";
  };

  async changeHandler(event) {
    const formData = { [event.target.name]: event.target.value };

    await this.setState({ ...this.state, ...formData });
  }

  SubmitHandler(event) {
    event.preventDefault();

    axios
      .post(
        API_ADDRESS + "UserJobShortDescription/EditUserShortDescription ",
        {
          id: this.state.info.id,
          jobTitle: this.state.info.jobTitle,
          employmentStatus: this.state.info.employmentStatus,
        },
        {
          headers: {
            Authorization: `bearer ${window.localStorage.getItem("JWT")}`,
          },
        }
      )
      .then((res) => toast.success("???? ???????????? ?????????? ????"))
      .catch(() => toast.error("?????????? ???? ???????? ?????????? ?????? ???????????? ???????????? ????????."))
      .finally(() => {
        this.cancel();
      });
  }

  returnGenderStatus = () => {
    var genderStatus = this.state.info2?.gender;

    if (genderStatus == 1) return "??????";
    if (genderStatus == 2) return "????";
  };

  returnMarridStatus = () => {
    var marridStatus = this.state.info2?.isMarreid;

    if (marridStatus) return "????????";
    if (!marridStatus) return "??????????";
  };

  returnSalary = () => {
    var status = this.state.info8?.salary;

    if (status === 0) return "???????? ???? 1 ????????????";
    if (status === 1) return "?????? 2.5 ???? 3.5 ????????????";
    if (status === 2) return "?????? 3.5 ???? 5 ????????????";
    if (status === 3) return "?????? 5 ???? 8 ????????????";
    if (status === 4) return "?????? 1 ???? 2.5 ????????????";
    if (status === 5) return "?????????? ???? ???? ????????????";
  };

  getSalary = () => {
    const value = this.state?.info8.salary;
    let lbl = "";
    for (let i = 0; i < salaries.length; i++) {
      if (salaries[i].value.toString() === value.toString()) {
        lbl = salaries[i].label;
        break;
      }
    }

    return lbl;
  };

  returnTypeOfCooperation = () => {
    var status = this.state.info8?.typeOfCooperation;

    if (status === 0) return "???????? ??????";
    if (status === 1) return "???????? ??????";
    if (status === 2) return "????????????????";
    if (status === 3) return "??????????????";
  };

  returnCategoryIds = (listOfData) => {
    var status = this.state.categories;

    let finalList = status?.map(({ value, label }) => {
      if (listOfData.includes(value)) {
        return label;
      }
    });
    return finalList;
  };

  returnSenioritylevel = () => {
    var status = this.state.info8?.senioritylevel;
    if (status == 0) return "?????? ????????";
    if (status == 1) return "???????? ???? ???? ??????";
    if (status == 2) return "?????????? 3 ???? 7 ??????";
    if (status == 3) return "?????????? ???? 7 ??????";
  };

  returnMarridStatus = () => {
    var marridStatus = this.state.info2?.isMarreid;

    if (!marridStatus) return "????????";
    if (marridStatus) return "??????????";
  };

  submitHandler = async (event) => {
    event.preventDefault();

    try {
      let { data } = await agent.CreateResome.editEmployeePersonalInformation(
        this.props.id
      );
      toast.success("?????????? ???? ???????????? ?????????? ????");
    } catch (err) {
      if (err.response.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response.status === 404) toast.error("???????? ???? ????????  ");
      else if (err.response.status === 500) toast.error("?????????? ???? ???????? ");
      else toast.error(err.response.data.message[0]);
    }
  };

  SubmitPersonalInfo = async (event) => {
    event.preventDefault();

    try {
      let data1 = { ...this.state.info2 };
      if (data1.isMarreid == "false") data1.isMarreid = false;
      if (data1.isMarreid == "true") data1.isMarreid = true;

      let { data } = await agent.CreateResome.editEmployeePersonalInformation(
        data1
      );
      toast.success("?????? ???????????? ????????");
      this.setState({ editMode1: false });
      this.cancel();
      this.getResomePercent();
    } catch (err) {
      if (err.response.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response.status === 404) toast.error("???????? ???? ????????  ");
      else if (err.response.status === 500) toast.error("?????????? ???? ???????? ");
      else {
        for (let index = 0; index < err.response.data.message.length; index++) {
          toast.error(err.response.data.message[index]);
        }
      }
      this.cancel();
    }
  };

  SubmitAboutMen = async (event) => {
    event.preventDefault();

    try {
      let obj = { aboutMe: this.state.aboutMen };
      let { data } = await agent.CreateResome.AddEmployeeAboutMen(obj);
      toast.success("?????? ???????????? ????????");
      this.setState({ editMode2: false });
      this.cancel();
      this.getResomePercent();
    } catch (err) {
      if (err.response?.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response.status === 404) toast.error("???????? ???? ????????  ");
      else if (err.response.status === 500) toast.error("?????????? ???? ???????? ");
      else {
        for (let index = 0; index < err.response.data.message.length; index++) {
          toast.error(err.response.data.message[index]);
        }
      }
      this.cancel();
    }
  };

  SubmitJobSkill = async (event) => {
    event.preventDefault();

    try {
      let currentJobSkill = this.state.currentJobSkill;
      let obj = { jobSkillId: currentJobSkill?.id };
      let { data } = await agent.CreateResome.AddUserJobSkill(obj);

      let userJobSkills = this.state.userJobSkills.concat({
        id: data.resul,
        jobSkillName: currentJobSkill.jobSkillName,
      });
      this.setState({
        userJobSkills,
        editMode4: false,
      });
      toast.success("?????? ???????????? ????????");
      this.getResomePercent();
    } catch (err) {
      if (err.response?.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response.status === 404) toast.error("???????? ???? ???????? ");
      else if (err.response.status === 500) toast.error("?????????? ???? ???????? ");
      else {
        for (let index = 0; index < err.response.data.message.length; index++) {
          toast.error(err.response.data.message[index]);
        }
      }
    }
  };

  SubmitJobPreference = async (event) => {
    event.preventDefault();

    let editProps = {
      ...this.state.info8,
      id: this.state.jobPreferenceStatus?.id,
    };
    let tempo = this.state.info8;
    delete tempo["id"];
    delete tempo["categoryForJobPrefence"];
    delete editProps["categoryForJobPrefence"];
    if (!tempo["categoryIds"]) {
      tempo["categoryIds"] = [];
      tempo["categoryIds"] = this.state.resuJobPreferenceIds;
    }
    if (!editProps["categoryIds"]) {
      editProps["categoryIds"] = [];
      editProps["categoryIds"] = this.state.resuJobPreferenceIds;
    }

    try {
      if (!this.state.jobPreferenceStatus) {
        await agent.CreateResome.AddUserJobPreference(tempo);
      } else {
        await agent.CreateResome.EditUserJobPreference(editProps);
      }

      this.setState({
        editMode8: false,
      });
      toast.success("?????? ???????????? ????????");
    } catch (err) {
      if (err.response?.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response?.status === 404) toast.error("???????? ???? ???????? ");
      else if (err.response?.status === 500) toast.error("?????????? ???? ???????? ");
      else {
        for (
          let index = 0;
          index < err?.response?.data?.message?.length;
          index++
        ) {
          toast.error(err.response.data.message[index]);
        }
      }
    }
  };

  DeleteJobPreference = async () => {
    const id = this.state.info8.id;

    Swal.fire({
      title: "?????? ?????????? ?????????? ???????????????? ?????? ??????????",
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#28a745",
      cancelButtonColor: "#d33",
      cancelButtonText: "??????",
      confirmButtonText: "?????? ?????????? ????????",
    }).then((result) => {
      if (result.value) {
        const fetchData = async () => {
          try {
            const data = await agent.CreateResome.DeleteUserJobPreference(id);

            this.setState({
              editMode8: false,
              info8: [],
            });
            Swal.fire({
              icon: "success",
              title: "???? ???????????? ?????? ????",
              showConfirmButton: false,
              timer: 1750,
            });
          } catch (err) {
            err?.response?.data?.message &&
              err.response.data.message.map((er) => toast.error(er));
          }
        };

        fetchData();
      }
    });
  };

  deleJobSkills = async (id) => {
    try {
      await agent.CreateResome.DeleteUserJobSkill(id);

      this.setState({
        userJobSkills: this.state.userJobSkills.filter(
          (item) => item.id !== id
        ),
      });

      toast.success("?????? ???????????? ????????");
    } catch (err) {
      if (err.response?.status === 401) toast.error("???????? ???????? ????????.");
      else if (err.response.status === 404) toast.error("???????? ???? ????????  ");
      else if (err.response.status === 500) toast.error("?????????? ???? ???????? ");
      else {
        for (let index = 0; index < err.response.data.message.length; index++) {
          toast.error(err.response.data.message[index]);
        }
      }
    }
  };

  onChangeSelectOption = (value, { action }) => {
    let tempArray = [];
    if (action === "select-option") {
      value &&
        value.map((x) => {
          tempArray.push(x.value);
        });
    } else if (action === "remove-value") {
      this.setState({
        info8: {
          ...this.state.info8,
          categoryIds: [],
        },
      });
      value &&
        value.map((x) => {
          tempArray.push(x.value);
        });
    } else if (action === "clear") {
      tempArray = [];
    }
    const listOfcategory = value?.map(({ label }) => {
      return {
        categoryName: label,
      };
    });
    console.log(listOfcategory);
    this.setState({
      info8: {
        ...this.state.info8,
        categoryIds: tempArray,
        categoryForJobPrefence: listOfcategory,
      },
    });
  };

  addItemToList = (value) => {
    this.setState({ getAllWorkExperience: value });
  };

  addItemToListEduBack = (value) => {
    this.setState({ getAllEduBackground: value });
  };

  addItemToListUserLanguage = (value) => {
    this.setState({ getAllLanguageForCurrentUser: value });
  };

  render() {
    return (
      <section className="container-fluid create-ad spx-2 spx-lg-10 smy-10 ">
        <h2 className="TitleOfCreateResume"> ?????????? ?????? </h2>
        <div className="row">
          <aside className="col-12 col-lg-8 smb-2 mb-lg-0">
            {/* aside and edit profile */}
            <h3 className="d-block text-right ir-b smb-3 c-dark">
              ?????????? ??????????????
            </h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12 col-lg-3 d-lg-flex flex-column justify-content-center">
                  <img
                    style={{ width: "212px" }}
                    className="w-100 d-block srounded-sm"
                    src={
                      avatarUrl + "/" + this.state.info2?.employeeAvatar ||
                      "/img/user-profile.png"
                    }
                    alt=""
                  />
                  <input
                    onChange={this.avatarHandler.bind(this)}
                    type="file"
                    className="d-none"
                    accept="image/x-png, image/jpeg"
                    id="avatar"
                  />
                  <label
                    htmlFor="avatar"
                    className="btn btn-light ir-r w-100 smt-1"
                  >
                    ?????????? ??????
                  </label>
                </div>

                <div className="col-12 col-lg-9">
                  {!this.state.editMode ? (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2"></h3>

                      <span
                        onClick={this.editDesc.bind(this)}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2">
                        ???????????? ??????????????
                      </h3>

                      <span
                        onClick={this.cancel.bind(this)}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <ul className="list-group list-group-flush p-0">
                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????? ?????? ????????????????:{" "}
                            <span className="c-regular">
                              {this.state.info
                                ? this.state.info.userFullName
                                : "-"}
                            </span>
                          </span>
                        </li>

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????????? ????????:{" "}
                            <span className="c-regular">
                              {this.state.info ? this.state.info.jobTitle : "-"}
                            </span>
                          </span>
                        </li>

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????????? ????????????:{" "}
                            <span className="c-regular">
                              {this.returnEmploymentStatus()}
                            </span>
                          </span>
                        </li>

                        <div className="editableContainer">
                          <li className="list-group-item border-0 pr-0">
                            <span className="ir-b c-grey sml-1">
                              ?????????? ????????:{" "}
                              <span className="c-regular">
                                {this.state.info
                                  ? this.state.info.lastCompanies
                                  : "-"}
                              </span>
                            </span>
                          </li>

                          <a
                            href="#workExperience"
                            className="editableContainerLink ir-r"
                          >
                            ????????????
                          </a>
                        </div>

                        <div className="editableContainer">
                          <li className="list-group-item border-0 pr-0">
                            <span className="ir-b c-grey sml-1">
                              ?????????? ???????? ????????????:{" "}
                              <span className="c-regular">
                                {this.state.info
                                  ? this.state.info.lastEducationBackground
                                  : "-"}
                              </span>
                            </span>
                          </li>

                          <a
                            href="#EducationBackground"
                            className="editableContainerLink ir-r"
                          >
                            ????????????
                          </a>
                        </div>
                      </ul>
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <form onSubmit={this.SubmitHandler.bind(this)}>
                        <ul className="list-group list-group-flush p-0">
                          <li className="list-group-item border-0 p-0">
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="fullName"
                              >
                                ?????? ?? ?????? ????????????????
                              </label>

                              <div className="form-group d-flex justify-content-center align-items-center">
                                <input
                                  disabled
                                  name="fullName"
                                  value={this.state.info.userFullName || ""}
                                  id="fullName"
                                  className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
                                  type="text"
                                  placeholder="????????: ?????? ?? ?????? ????????????????"
                                />
                              </div>
                            </div>
                          </li>

                          <li className="list-group-item border-0 p-0">
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ?????????? ????????
                              </label>

                              <div className="form-group d-flex justify-content-center align-items-center">
                                <input
                                  onChange={(e) => {
                                    this.setState({
                                      info: {
                                        ...this.state.info,
                                        jobTitle: e.target.value,
                                      },
                                    });
                                  }}
                                  name="jobTitle"
                                  value={this.state.info.jobTitle || ""}
                                  id="jobTitle"
                                  className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
                                  type="text"
                                  placeholder="????????: ???????????? ???????? ?????????? ??????"
                                />
                              </div>
                            </div>
                          </li>

                          <li className="list-group-item border-0 p-0">
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="employmentStatus"
                              >
                                ?????????? ????????????
                              </label>

                              <div>
                                <div className="custom-control custom-radio custom-control-inline">
                                  <input
                                    onChange={this.radionHandler.bind(this)}
                                    checked={
                                      this.state.info.employmentStatus == 1
                                    }
                                    type="radio"
                                    value="1"
                                    id="status1"
                                    name="status"
                                    className="custom-control-input"
                                  />
                                  <label
                                    className="custom-control-label ir-r"
                                    htmlFor="status1"
                                  >
                                    ?????????? ??????
                                  </label>
                                </div>

                                <div className="custom-control custom-radio custom-control-inline">
                                  <input
                                    onChange={this.radionHandler.bind(this)}
                                    checked={
                                      this.state.info.employmentStatus == 2
                                    }
                                    type="radio"
                                    value="2"
                                    id="status2"
                                    name="status"
                                    className="custom-control-input"
                                  />
                                  <label
                                    className="custom-control-label ir-r"
                                    htmlFor="status2"
                                  >
                                    ????????
                                  </label>
                                </div>

                                <div className="custom-control custom-radio custom-control-inline">
                                  <input
                                    onChange={this.radionHandler.bind(this)}
                                    checked={
                                      this.state.info.employmentStatus == 3
                                    }
                                    type="radio"
                                    value="3"
                                    id="status3"
                                    name="status"
                                    className="custom-control-input"
                                  />
                                  <label
                                    className="custom-control-label ir-r"
                                    htmlFor="status3"
                                  >
                                    ???? ?????????? ?????? ????????
                                  </label>
                                </div>
                              </div>
                            </div>
                          </li>
                        </ul>

                        <button
                          type="submit"
                          className="btn btn-success ir-r fs-m smt-2"
                        >
                          ??????????
                        </button>
                      </form>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* pdf */}
            <UploadPdf />

            {/* </aside> */}
            <h3 className="d-block text-right ir-b smb-3 c-dark">
              ?????????????? ????????
            </h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12">
                  {!this.state.editMode2 ? (
                    <header className="d-flex justify-content-between align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode2: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r "
                      >
                        ????????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2">
                        ???????????? ??????????????
                      </h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode2: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode2 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <ul className="list-group list-group-flush p-0">
                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ??????????:{" "}
                            <span className="c-regular">
                              {this.state.info2 ? this.state.info2?.email : "-"}
                            </span>
                          </span>
                        </li>

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????? :{" "}
                            <span className="c-regular">
                              {this.state.info2 ? this.state.info2?.city : "-"}
                            </span>
                          </span>
                        </li>

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????????? ????????????:{" "}
                            <span className="c-regular">
                              {this.state.info2
                                ? this.state.info2?.phoneNumber
                                : "-"}
                            </span>
                          </span>
                        </li>

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ???????? ?????? ?????????? (??????????????) :{" "}
                            <span className="c-regular">
                              {this.state.info2
                                ? this.state.info2?.address
                                : "-"}
                            </span>
                          </span>
                        </li>

                        {this.state.info2?.gender !== 2 && (
                          <li className="list-group-item border-0 pr-0">
                            <span className="ir-b c-grey sml-1">
                              ?????????? ???????? ??????????:{" "}
                              <span className="c-regular">
                                {this.state.info2
                                  ? this.state.info2?.military
                                  : "-"}
                              </span>
                            </span>
                          </li>
                        )}

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????????? :{" "}
                            <span className="c-regular">
                              {this.returnGenderStatus()}
                            </span>
                          </span>
                        </li>

                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ?????????? ???????? :{" "}
                            <span className="c-regular">
                              {this.returnMarridStatus()}
                            </span>
                          </span>
                        </li>

                        {this.state.info2?.gender !== 2 && (
                          <>
                            <li className="list-group-item border-0 pr-0">
                              <span className="ir-b c-grey sml-1">
                                ?????????? ???????????? :
                                <span className="c-regular">
                                  {this.state.info2?.exemptionExpirestionDate
                                    ? this.state.info2?.exemptionExpirestionDate
                                    : "-"}
                                </span>
                              </span>
                            </li>

                            <li className="list-group-item border-0 pr-0">
                              <span className="ir-b c-grey sml-1">
                                ?????????? ???????????? ???????? ????????????:
                                <span className="c-regular">
                                  {this.state.info2
                                    ?.exemptionExpirestionRecieveDate
                                    ? this.state.info2
                                      ?.exemptionExpirestionRecieveDate
                                    : "-"}
                                </span>
                              </span>
                            </li>
                          </>
                        )}
                      </ul>
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <form onSubmit={this.SubmitPersonalInfo.bind(this)}>
                        <ul className="list-group list-group-flush p-0">
                          <li className="list-group-item border-0 p-0">
                            <InputText
                              type="email"
                              id="email"
                              name="email"
                              label={"??????????"}
                              value={this.state.info2?.email || ""}
                              onChange={(e) => {
                                this.setState({
                                  info2: {
                                    ...this.state.info2,
                                    email: e.target.value,
                                  },
                                });
                              }}
                              placeholder="????????:sia@gmail.com"
                            />
                          </li>

                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ??????
                              </label>

                              <div className="form-group ">
                                <Select
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        city: e.value,
                                      },
                                    });
                                  }}
                                  placeholder="???????????? ??????"
                                  value={this.state.cities.map((item) => {
                                    if (
                                      this.state.info2?.city === item.cityName
                                    ) {
                                      return {
                                        value: item.cityName,
                                        label: ` ${item.provinceName}?? ${item.cityName} `,
                                      };
                                    }
                                  })}
                                  styles={{ fontFamily: "iransans-regular" }}
                                  options={this.state.cities.map((item) => {
                                    return {
                                      value: item.cityName,
                                      label: ` ${item.provinceName}?? ${item.cityName} `,
                                    };
                                  })}
                                />
                              </div>
                            </div>
                          </li>

                          <li className="list-group-item border-0 p-0">
                            <InputText
                              type="address"
                              id="text"
                              name="address"
                              label={"?????????? ????????????"}
                              value={this.state.info2?.phoneNumber || ""}
                              onChange={(e) => {
                                this.setState({
                                  info2: {
                                    ...this.state.info2,
                                    phoneNumber: e.target.value,
                                  },
                                });
                              }}
                              placeholder="????????:09115674333"
                            />
                          </li>

                          <li className="list-group-item border-0 p-0">
                            <InputText
                              type="string"
                              id="address"
                              name="address"
                              label={"????????"}
                              value={this.state.info2?.address || ""}
                              onChange={(e) => {
                                this.setState({
                                  info2: {
                                    ...this.state.info2,
                                    address: e.target.value,
                                  },
                                });
                              }}
                              placeholder="????????: ???????? ???????????? ??????????"
                            />
                          </li>

                          <li className="list-group-item border-0 p-0 mb-3">
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right label bg-white"
                                htmlFor="employmentStatus"
                              >
                                ?????????? ????????
                              </label>

                              <div>
                                <InputRadio
                                  checked={
                                    this.state.info2?.isMarreid == "true" ||
                                    this.state.info2?.isMarreid == true
                                  }
                                  label={"??????????"}
                                  type="radio"
                                  value={true}
                                  id="status20"
                                  name="status20"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        isMarreid: e.target.value,
                                      },
                                    });
                                  }}
                                />
                                <InputRadio
                                  checked={
                                    this.state.info2?.isMarreid == "false" ||
                                    this.state.info2?.isMarreid == false
                                  }
                                  label={"????????"}
                                  type="radio"
                                  value={false}
                                  id="status21"
                                  name="status21"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        isMarreid: e.target.value,
                                      },
                                    });
                                  }}
                                />
                              </div>
                            </div>
                          </li>

                          <li className="list-group-item border-0 p-0 mb-3">
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right  label bg-white"
                                htmlFor="employmentStatus"
                              >
                                ??????????
                              </label>

                              <div>
                                <InputRadio
                                  checked={
                                    this.state.info2?.gender == 1 ||
                                    this.state.info2?.gender == 1
                                  }
                                  label={"??????"}
                                  type="radio"
                                  value={true}
                                  id="status22"
                                  name="status22"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        gender: 1,
                                      },
                                    });
                                  }}
                                />
                                <InputRadio
                                  checked={
                                    this.state.info2?.gender == 2 ||
                                    this.state.info2?.gender == 2
                                  }
                                  label={"????"}
                                  type="radio"
                                  value={false}
                                  id="status4"
                                  name="status4"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        gender: 2,
                                        exemptionExpirestionDate: "",
                                        exemptionExpirestionRecieveDate: "",
                                      },
                                    });
                                  }}
                                />
                              </div>
                            </div>
                          </li>
                          {this.state?.info2?.gender === 1 && (
                            <>
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="employmentStatus"
                              >
                                ?????????? ????????
                              </label>
                              <div>
                                <InputRadio
                                  checked={
                                    this.state.info2?.military === "??????????"
                                  }
                                  label={"??????????"}
                                  type="radio"
                                  id="status10"
                                  name="status10"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        military: "??????????",
                                        exemptionExpirestionDate: "",
                                        exemptionExpirestionRecieveDate: "",
                                      },
                                    });
                                  }}
                                />
                                <InputRadio
                                  checked={
                                    this.state.info2?.military === "????????"
                                  }
                                  label={"????????"}
                                  type="radio"
                                  id="status11"
                                  name="status11"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        military: "????????",
                                      },
                                    });
                                  }}
                                />
                                <InputRadio
                                  checked={
                                    this.state.info2?.military ===
                                    "?????????? ???????? ?????????? ????????"
                                  }
                                  label={"?????????? ???????? ?????????? ????????"}
                                  type="radio"
                                  id="status12"
                                  name="status12"
                                  onChange={(e) => {
                                    this.setState({
                                      info2: {
                                        ...this.state.info2,
                                        military: "?????????? ???????? ?????????? ????????",
                                      },
                                    });
                                  }}
                                />
                              </div>
                            </>
                          )}

                          {this.state.info2.gender === 1 &&
                            this.state.info2.military !== "??????????" && (
                              <>
                                <li className="list-group-item border-0 p-0 mt-3">
                                  <label className="ir-r text-regular text-right smb-1 label bg-white">
                                    ?????????? ????????????
                                  </label>
                                  <div className="form-group mb-0 ">
                                    <DatePickerModern
                                      handleChange={(e) => {
                                        this.setState({
                                          info2: {
                                            ...this.state.info2,
                                            exemptionExpirestionDate: e,
                                          },
                                        });
                                      }}
                                      dateVal={
                                        this.state.info2
                                          .exemptionExpirestionDate &&
                                        this.state.info2
                                          .exemptionExpirestionDate
                                      }
                                      name="exemptionExpirestionDate"
                                    />
                                  </div>
                                </li>
                                <li className="list-group-item border-0 p-0">
                                  <label className="ir-r text-regular text-right smb-1 label bg-white">
                                    ?????????? ???????????? ???????? ????????????
                                  </label>
                                  <div className="form-group mb-0 ">
                                    <DatePickerModern
                                      handleChange={(e) => {
                                        this.setState({
                                          info2: {
                                            ...this.state.info2,
                                            exemptionExpirestionRecieveDate: e,
                                          },
                                        });
                                      }}
                                      dateVal={
                                        this.state.info2
                                          .exemptionExpirestionRecieveDate &&
                                        this.state.info2
                                          .exemptionExpirestionRecieveDate
                                      }
                                      name="exemptionExpirestionRecieveDate"
                                    />
                                  </div>
                                </li>
                              </>
                            )}
                        </ul>

                        <button
                          type="submit"
                          className="btn btn-success ir-r fs-m smt-2"
                        >
                          ??????????
                        </button>
                      </form>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* </aside> */}
            <h3 className="d-block text-right ir-b smb-3 c-dark">???????????? ????</h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12">
                  {!this.state.editMode3 ? (
                    <header className="d-flex justify-content-between align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode3: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2">
                        ???????????? ??????????????
                      </h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode3: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode3 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <ul className="list-group list-group-flush p-0">
                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1 ">
                            ???????????? ????:
                            <p className="c-regular text-break m-0">
                              {this.state.aboutMen ? this.state?.aboutMen : "-"}
                            </p>
                          </span>
                        </li>
                      </ul>
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <form onSubmit={this.SubmitAboutMen.bind(this)}>
                        <ul className="list-group list-group-flush p-0">
                          <li className="list-group-item border-0 p-0">
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ???????????? ????
                              </label>

                              <div className="form-group d-flex justify-content-center align-items-center">
                                <textarea
                                  style={{ minHeight: "200px" }}
                                  onChange={(e) => {
                                    this.setState({
                                      aboutMen: e.target.value,
                                    });
                                  }}
                                  name="aboutMen"
                                  value={this.state?.aboutMen || ""}
                                  className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
                                  type="textarea"
                                  placeholder="???? ?????????? ?? ??????????????????? ??????????????? ?? ???????? ?????? ??????????????..."
                                />
                              </div>
                            </div>
                          </li>
                        </ul>
                        <button
                          type="submit"
                          className="btn btn-success ir-r fs-m smt-2"
                        >
                          ??????????
                        </button>
                      </form>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* </aside> */}
            <h3 className="d-block text-right ir-b smb-3 c-dark">
              ??????????????????? ???????????????
            </h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12 col-lg-12">
                  {!this.state.editMode4 ? (
                    <header className="d-flex justify-content-between align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode4: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2">
                        ???????????? ??????????????
                      </h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode4: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode4 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <ul className="list-group list-group-flush p-0">
                        <li className="list-group-item border-0 pr-0">
                          <span className="ir-b c-grey sml-1">
                            ??????????????????? ??????????????? :
                          </span>
                        </li>
                      </ul>
                      <div className="p-1">
                        {this.state.userJobSkills?.map((item) => {
                          return (
                            <button
                              key={item.id}
                              className="btn btn-success ml-1 m-1"
                            >
                              {item.jobSkillName}
                              <i
                                className="mr-1 fas fa-times"
                                onClick={() => {
                                  this.deleJobSkills(item.id);
                                }}
                                style={{ cursor: "default" }}
                              ></i>
                            </button>
                          );
                        })}
                      </div>
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <form onSubmit={this.SubmitJobSkill.bind(this)}>
                        <ul className="list-group list-group-flush p-0">
                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ??????????????????? ???????????????
                              </label>

                              <div className="form-group ">
                                <Select
                                  // userJobSkills: resuserJobSkill.data.resul,

                                  onChange={(e) => {
                                    // console.clear();
                                    // let userJobSkills = this.state.userJobSkills.concat({ id: e.value, jobSkillName: e.label });
                                    this.setState({
                                      currentJobSkill: {
                                        id: e.value,
                                        jobSkillName: e.label,
                                      },
                                    });
                                  }}
                                  placeholder="?????????? ??????..."
                                  styles={{ fontFamily: "iransans-regular" }}
                                  // options={this.state.cities}

                                  options={this.state?.jobSkills?.map(
                                    (item) => {
                                      return {
                                        value: item.id,
                                        label: `${item.name}`,
                                      };
                                    }
                                  )}
                                />
                              </div>
                            </div>
                          </li>
                        </ul>
                        <button
                          type="submit"
                          className="btn btn-success ir-r fs-m smt-2"
                        >
                          ??????????
                        </button>
                      </form>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* job Preference */}
            <h3 className="d-block text-right ir-b smb-3 c-dark">
              ?????????????? ????????
            </h3>
               <JobPreferences getResomePercent={this.getResomePercent} />                       
            {/* <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12">
                  {!this.state.editMode8 ? (
                    <header className="d-flex justify-content-flex-start align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode8: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2">
                        ???????????? ??????????????
                      </h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode8: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode8 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <JobPreferenceDetails
                        AllWorkExperience={this.state.info8}
                      />
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <form onSubmit={this.SubmitJobPreference.bind(this)}>
                        <ul className="list-group list-group-flush p-0">
                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ??????
                              </label>

                              <Autocomplete
                                id="city-select"
                                className="mb-2 pr-0"
                                style={{ width: "100%", height: "40px" }}
                                options={this.state.cities.map((item) => {
                                  return {
                                    value: item.cityName,
                                    label: ` ${item.provinceName}?? ${item.cityName} `,
                                  };
                                })}
                                getOptionLabel={(option) => option.label}
                                renderOption={(option) =>
                                  <React.Fragment>
                                    {option.label}
                                  </React.Fragment>
                                }
                                onInputChange={(event, newInputValue) => {
                                  const info8 = { ...this.state?.info8 };
                                  info8.city = newInputValue;
                                  this.setState({ info8 }, () => { console.log(newInputValue) });
                                }}
                                defaultValue={this.state.info8?.city}
                                renderInput={(params) => (
                                  <TextField
                                    {...params}
                                    // label="Choose a country"
                                    variant="outlined"

                                  />
                                )}
                              />
                            </div>
                          </li>

                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 mt-4 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ?????????? ????????
                              </label>
                              <Autocomplete
                               

                                id="salary-select"
                                className="mb-2 pr-0"
                                style={{ width: "100%", height: "40px" }}
                                options={salaries}
                                getOptionLabel={(option) => option.label}
                                renderOption={(option) =>
                                  <React.Fragment>
                                    {option.label}
                                  </React.Fragment>
                                }
                                onInputChange={ async (event, newInputValue) => {

                                  const info8 = { ...this.state?.info8 };
                                  await salaries.every(item => {
                                    if (item.label === newInputValue.trim()) {
                                      info8.salary = item.value;
                                      return false;
                                    }
                                    return true;
                                  });
                                  this.setState({ info8 }, () => { console.log(newInputValue) });
                                }}
                                defaultValue={salaries[0]}
                                renderInput={(params) => (
                                  <TextField
                                    {...params}
                                    // label="Choose a country"
                                    variant="outlined"

                                  />
                                )}
                              />

                            </div>
                          </li>

                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 mt-4 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ?????? ????????????
                              </label>
                              <Autocomplete

                                id="city-select"
                                className="mb-2 pr-0"
                                style={{ width: "100%", height: "40px" }}
                                options={typeOfCooperation}
                                getOptionLabel={(option) => option.label}
                                renderOption={(option) =>
                                  <React.Fragment>
                                    {option.label}
                                  </React.Fragment>
                                }
                                onInputChange={async (event, newInputValue) => {

                                  const info8 = { ...this.state?.info8 };
                                  await typeOfCooperation.every(item => {
                                    if (item.label === newInputValue.trim()) {
                                      info8.typeOfCooperation = item.value;
                                      return false;
                                    }
                                    return true;
                                  });
                                  
                                  this.setState({ info8 }, () => { console.log(newInputValue) });
                                }}
                                defaultValue={
                                  {
                                    value: this.state.info8?.typeOfCooperation,
                                    label: this.returnTypeOfCooperation(),
                                  }
                                }
                                renderInput={(params) => (
                                  <TextField
                                    {...params}
                                    // label="Choose a country"
                                    variant="outlined"

                                  />
                                )}
                              />
                             
                            </div>
                          </li>

                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 mt-4 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ???????? ???????? ????????
                              </label>
                              <div className="form-group ">
                                <Select
                                  defaultValue={this.state.info8?.categoryForJobPrefence?.map(
                                    ({ categoryId, categoryName }) => ({
                                      value: categoryId,
                                      label: categoryName,
                                    })
                                  )}
                                  onChange={this.onChangeSelectOption}
                                  isSearchable={false}
                                  placeholder={" ???????? ???????? ????????"}
                                  options={this.state.categories}
                                  styles={{ fontFamily: "iransans-regular" }}
                                  isMulti
                                />
                              </div>
                            </div>
                          </li>

                          <li>
                            <div className="text-input srounded-sm">
                              <label
                                className="ir-r text-regular text-right smb-1 label bg-white"
                                htmlFor="jobTitle"
                              >
                                ?????????? ??????
                              </label>
                              <Autocomplete
                                id="resume-select"
                                className="mb-2 pr-0"
                                style={{ width: "100%", height: "40px" }}
                                options={expriences}
                                getOptionLabel={(option) => option.label}
                                renderOption={(option) =>
                                  <React.Fragment>
                                    {option.label}
                                  </React.Fragment>
                                }
                                styles={{ fontFamily: "iransans-regular" }}
                                onInputChange={async (event, newInputValue) => {

                                  const info8 = { ...this.state?.info8 };
                                  await expriences.every(item => {
                                    if (item.label === newInputValue.trim()) {
                                      info8.senioritylevel = item.value;
                                      return false;
                                    }
                                    return true;
                                  });

                                  this.setState({ info8 }, () => { console.log(newInputValue) });
                                }}
                                defaultValue={
                                  {
                                    value: this.state.info8?.senioritylevel,
                                    label: this.returnSenioritylevel(),
                                  }
                                }
                                renderInput={(params) => (
                                  <TextField
                                    {...params}
                                    // label="Choose a country"
                                    variant="outlined"

                                  />
                                )}
                              />
                            
                            </div>
                          </li>

                          <li>
                            <div className="custom-control custom-checkbox custom-control-inline mt-5">
                              <input
                                checked={
                                  this.state.info8?.insurance === "true" ||
                                  this.state.info8?.insurance === true
                                }
                                onChange={(e) => {
                                  this.setState({
                                    info8: {
                                      ...this.state.info8,
                                      insurance: e.target.checked,
                                    },
                                  });
                                }}
                                type="checkbox"
                                id="status40"
                                name="status40"
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label ir-r"
                                htmlFor="status40"
                              >
                                ????????
                              </label>
                            </div>
                            <div className="custom-control custom-checkbox custom-control-inline">
                              <input
                                checked={
                                  this.state.info8?.promotion === "true" ||
                                  this.state.info8?.promotion === true
                                }
                                onChange={async (e) => {
                                  await this.setState({
                                    info8: {
                                      ...this.state.info8,
                                      promotion: e.target.checked,
                                    },
                                  });
                                }}
                                type="checkbox"
                                id="status41"
                                name="status41"
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label ir-r"
                                htmlFor="status41"
                              >
                                ?????????? ????????
                              </label>
                            </div>
                            <div className="custom-control custom-checkbox custom-control-inline">
                              <input
                                checked={
                                  this.state.info8?.flexibleWorkingTime ===
                                  "true" ||
                                  this.state.info8?.flexibleWorkingTime === true
                                }
                                onChange={async (e) => {
                                  await this.setState({
                                    info8: {
                                      ...this.state.info8,
                                      flexibleWorkingTime: e.target.checked,
                                    },
                                  });
                                }}
                                type="checkbox"
                                id="status42"
                                name="status42"
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label ir-r"
                                htmlFor="status42"
                              >
                                ???????????? ???????? ???????? ???????? ????????
                              </label>
                            </div>
                            <div className="custom-control custom-checkbox custom-control-inline">
                              <input
                                checked={
                                  this.state.info8?.hasMeel === "true" ||
                                  this.state.info8?.hasMeel === true
                                }
                                onChange={async (e) => {
                                  await this.setState({
                                    info8: {
                                      ...this.state.info8,
                                      hasMeel: e.target.checked,
                                    },
                                  });
                                }}
                                type="checkbox"
                                id="status43"
                                name="status43"
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label ir-r"
                                htmlFor="status43"
                              >
                                ?????????? ???? ???????? ??????????
                              </label>
                            </div>
                            <div className="custom-control custom-checkbox custom-control-inline">
                              <input
                                checked={
                                  this.state.info8?.transportationService ===
                                  "true" ||
                                  this.state.info8?.transportationService ===
                                  true
                                }
                                onChange={async (e) => {
                                  await this.setState({
                                    info8: {
                                      ...this.state.info8,
                                      transportationService: e.target.checked,
                                    },
                                  });
                                }}
                                type="checkbox"
                                id="status44"
                                name="status44"
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label ir-r"
                                htmlFor="status44"
                              >
                                ?????????? ???? ?????????? ?????? ?? ??????
                              </label>
                            </div>
                            <div className="custom-control custom-checkbox custom-control-inline">
                              <input
                                checked={
                                  this.state.info8?.educationCourses ===
                                  "true" ||
                                  this.state.info8?.educationCourses === true
                                }
                                onChange={async (e) => {
                                  await this.setState({
                                    info8: {
                                      ...this.state.info8,
                                      educationCourses: e.target.checked,
                                    },
                                  });
                                }}
                                type="checkbox"
                                id="status45"
                                name="status45"
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label ir-r"
                                htmlFor="status45"
                              >
                                ???????? ????????????
                              </label>
                            </div>
                          </li>
                        </ul>

                        <button
                          type="submit"
                          className="btn btn-success ir-r fs-m smt-2"
                        >
                          ??????????
                        </button>
                      </form>
                    </div>
                  )}
                </div>
              </div>
            </div> */}

            {/* job Experience */}
            <span id="workExperience"></span>
            <h3 className="d-block text-right ir-b smb-3 c-dark ">
              ?????????? ????????
            </h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12">
                  {!this.state.editMode9 ? (
                    <header className="d-flex justify-content-between align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode9: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ?????? ?????????? ????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2"></h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode9: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode9 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <JobExpreinceDetails
                        AllWorkExperience={this.state.getAllWorkExperience}
                      />
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <JobExpreinceFormGenerator
                        AllWorkExperience={this.state.getAllWorkExperience}
                        addItemToList={this.addItemToList}
                      />
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* education */}
            <span id="EducationBackground"></span>
            <h3 className="d-block text-right ir-b smb-3 c-dark">??????????????</h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12">
                  {!this.state.editMode10 ? (
                    <header className="d-flex justify-content-between align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode10: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ?????? ??????????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2"></h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode10: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode10 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <EducationalBackgroundDetails
                        AllEduBackground={this.state.getAllEduBackground}
                        GetAllLanguages={this.state.getAllLanguages}
                      />
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <EducationalBackgroundFormGenerator
                        AllEduBackground={this.state.getAllEduBackground}
                        addItemToList={this.addItemToListEduBack}
                        GetAllLanguages={this.state.getAllLanguages}
                      />
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* language */}
            <h3 className="d-block text-right ir-b smb-3 c-dark">???????? ????</h3>
            <div className="bg-white srounded-md sp-2 smb-3">
              <div className="row">
                <div className="col-12">
                  {!this.state.editMode11 ? (
                    <header className="d-flex justify-content-between align-items-center flex-row-reverse">
                      <span
                        onClick={() => {
                          this.setState({ editMode11: true });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ?????? ????????
                      </span>
                    </header>
                  ) : (
                    <header className="d-flex justify-content-between align-items-center">
                      <h3 className="ir-b c-primary text-right d-block fs-m smb-2"></h3>

                      <span
                        onClick={() => {
                          this.setState({ editMode11: false });
                        }}
                        type="button"
                        className="btn btn-info ir-r"
                      >
                        ????????????
                      </span>
                    </header>
                  )}

                  {!this.state.editMode11 ? (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <LanguageDetails
                        getAllLanguageForCurrentUser={
                          this.state.getAllLanguageForCurrentUser
                        }
                      />
                    </div>
                  ) : (
                    <div className="content d-lg-flex flex-column justify-content-center">
                      <LanguageGenerator
                        getAllLanguageForCurrentUser={
                          this.state.getAllLanguageForCurrentUser
                        }
                        addItemToList={this.addItemToListUserLanguage}
                      />
                    </div>
                  )}
                </div>
              </div>
            </div>
          </aside>

          <aside className="col-12 col-lg-4">
            <SideBar resomePercent={this.state.resomePercent} />
          </aside>
        </div>
      </section>
    );
  }
}

const InputText = ({ value, name, placeHolder, label, onChange, ...rest }) => {
  return (
    <div className="text-input srounded-sm">
      <label
        className="ir-r text-regular text-right smb-1 label bg-white"
        htmlFor="jobTitle"
      >
        {label}
      </label>

      <div className="form-group d-flex justify-content-center align-items-center">
        <input
          onChange={onChange}
          name={name}
          value={value || ""}
          className="form-control digit d-block fs-m text-right ir-r text-regular shadow-none"
          type="text"
          placeholder={placeHolder}
        />
      </div>
    </div>
  );
};

const InputRadio = ({
  value,
  name,
  placeHolder,
  label,
  onChange,
  checked,
  id,
  ...rest
}) => {
  return (
    <>
      <div className="custom-control custom-radio custom-control-inline">
        <input
          onChange={onChange}
          checked={checked}
          type="radio"
          value={value}
          id={id}
          name={name}
          className="custom-control-input"
        />
        <label className="custom-control-label ir-r" htmlFor={id}>
          {label}
        </label>
      </div>
    </>
  );
};
