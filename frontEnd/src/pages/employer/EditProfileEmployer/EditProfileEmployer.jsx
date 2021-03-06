import React, { useEffect, useState } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import { useHistory } from "react-router-dom";
import { Formik, Field, Form, ErrorMessage, useField } from "formik";
import CKEditor from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import Select from "react-select";

import { EditProfileEmployerVal } from "../../../core/validation/editProfileEmployer";
import ADDRESS from "../../../ADDRESS";
import API_ADDRESS from "../../../API_ADDRESS";
import parse from 'react-html-parser';
import { MiniSpinner } from "../../../components/spinner/MiniSpinner";
import "./../Field.style.css";




const EmployersDefaultLogo = `${ADDRESS}Img/CompanyLogo/defult-employer-logo.jpg`

// import EmployersDefaultLogo from '//panel.katinojob.ir/Img/CompanyLogo/defult-employer-logo.jpg';


const EditProfileEmployer = () => {
  const initialD = {
    ManagmentFullName: "",
    PersianFullName: "",
    EngFullName: "",
    EmergencPhone: "",
    Image: "",
    url: "",
    FieldOfActivity: "",
    NumberOfStaff: 0,
    Email: "",
    City: "",
    ShortDescription: "",
    FieldOfActivity2: [],
  };

  const [initialData, setInitialData] = useState(initialD);
  const [fieldOptions, setFieldOptions] = useState("");
  const [cities, setCities] = useState();
  const [profilePicUrl, setProfilePicUrl] = useState("");
  const [PicUrl, setPicUrl] = useState("");
  const [imageError, setImageError] = useState("");
  const [selectedValue, setSelectedValue] = useState([]);
  const [loading, setLoading] = useState(false);

  const history = useHistory();

  useEffect(() => {

    const circle_symbol = parse('&#9679');
    const dash_symbol = parse('&#8211');
    const space_symbol = parse("&nbsp;");

    const getData = async () => {
      setLoading(true);

      let fieldOptions = [];
      let cities = [];
      await axios.get(API_ADDRESS + "Categories/GetAllCategories2").then((res) => {
        res.data.resul.forEach((item, index) => {
          if (item?.children.length > 0) {
            fieldOptions.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
            item.children.forEach(item_child => {
              fieldOptions.push({ value: item_child.id, label: space_symbol + space_symbol + space_symbol + dash_symbol + space_symbol + item_child.name });
            })
          }
          else {
            fieldOptions.push({ value: item.id, label: circle_symbol + space_symbol + item.name });
          }
        });
      });
      setFieldOptions(fieldOptions);

      await axios.get(API_ADDRESS + "Account/GetCities").then((res) => {
        res.data.resul.map((item) => {
          cities.push({
            value: item.cityDivisionCode,
            label: `${item.provinceName}?? ${item.cityName}`,
          });
        });
        setCities(cities);
        setLoading(false);
      });


    };
    getData();
  }, []);

  useEffect(() => {
    axios.get(API_ADDRESS + "Account/LoadEmployerProfile").then((res) => {
      let fieldOfActivityData = res.data.resul.fieldOfActivity?.map(
        (item) => item.id
      );
      setSelectedValue(fieldOfActivityData);
      console.log(res.data.resul.fieldOfActivity);

      setInitialData({
        ManagmentFullName: res.data.resul.managmentFullName || "",
        PersianFullName: res.data.resul.persianFullName || "",
        EngFullName: res.data.resul.engFullName || "",
        EmergencPhone: res.data.resul.emergencPhone || "",
        Image: res.data.resul.image ? `${ADDRESS}img/CompanyLogo/${res.data.resul.image}` : "",
        url: res.data.resul.url || "",
        FieldOfActivity: fieldOfActivityData,
        NumberOfStaff: res.data.resul.numberOfStaff || 0,
        Email: res.data.resul.email || "",
        City: parseInt(res.data.resul.cities) || "",
        ShortDescription: res.data.resul.shortDescription || "",
      });
    });
  }, []);

  const convertObjToFormData = (obj) => {
    const fd = new FormData();
    for (let key in obj) {
      if (key !== "FieldOfActivity") {
        fd.append(key, obj[key]);
      }
    }

    obj["FieldOfActivity"].map((e) => {
      fd.append("FieldOfActivity", e);
    });

    return fd;
  };

  const preview = (files) => {
    setImageError("");
    if (files?.length === 0) {
      setProfilePicUrl(null);
      return;
    }

    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      setImageError("???????? ???????? ???? ???????? ????????");
      return;
    }

    setPicUrl(files[0]);
    var reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = () => {
      setProfilePicUrl(reader.result);
    };
  };

  const handleValid = (e) => {
    /*
    ManagmentFullName: "",
    PersianFullName: "",
    EngFullName: "",
    EmergencPhone: "",
    Image: "",
    url: "",
    FieldOfActivity: "",
    NumberOfStaff: 0,
    Email: "",
    City: "",
    ShortDescription: "",
    FieldOfActivity2: [],
    */
    // e.preventDefault();
    console.log(e)

    // if(values.ManagmentFullName.trim() === "" || values.PersianFullName.trim() === "" || values.EngFullName.trim() === ""
    // || values.EmergencPhone.trim() === ""  || values.FieldOfActivity.trim() === ""
    // ){
    //   toast.error("???????? ?????? ?? ?????? ???????????????? ???????????? ???????? ???? ???????? ???????? .")
    // }


  };

  const submitHandler = (values) => {

    console.log(values)

    setLoading(true);

    let tempo = { ...values, Image: PicUrl };
    const correctFormat = convertObjToFormData(tempo);

    axios
      .post(API_ADDRESS + "Account/EditEmployerProfile", correctFormat, {
        headers: {
          Authorization: `bearer ${localStorage.getItem("JWT")}`,
        },
      })
      .then((res) => {
        toast.success("???????????? ?????????????? ???? ???????????? ?????????? ????");

        // Swal.fire({
        //   icon: "success",
        //   title: "???????????? ?????????????? ???? ???????????? ?????????? ????",
        //   showConfirmButton: false,
        //   timer: 1750,
        // });

        setLoading(false);
      })
      .catch((err) => {
        if (err.response.status === 400 && err.response) {
          err.response.data.message.map((e) => {
            toast.error(e);
          });
        }

        setLoading(false);
      });
  };

  const MultiMySelect = ({ label, options, ...props }) => {
    const [field, , helpers] = useField(props);

    const onChangeSelectOption = (value, { action }) => {
      let tempArray = [];
      if (action === "select-option") {
        value &&
          value.map((x) => {
            tempArray.push(x.value);
          });
      } else if (action === "remove-value") {
        setSelectedValue([]);
        value &&
          value.map((x) => {
            tempArray.push(x.value);
          });
      } else if (action === "clear") {
        tempArray = [];
      }

      helpers.setValue(tempArray);
      setSelectedValue(tempArray);
    };

    return (
      <div>
        <Select
          {...field}
          {...props}
          options={options}
          onChange={onChangeSelectOption}
          value={
            options
              ? options.filter((obj) => selectedValue.includes(obj.value))
              : []
          }
          className="basic-multi-select"
          classNamePrefix="select"
          isMulti
        />
        <ErrorMessage
          component="div"
          className="errorMessage"
          name={props.name}
        />
      </div>
    );
  };

  const MySelect = ({ label, options, ...props }) => {
    const [field, , helpers] = useField(props);
    return (
      <div>
        <Select
          {...field}
          {...props}
          options={options}
          value={
            options
              ? options.find((option) => option.value === field.value)
              : ""
          }
          onChange={(option) => helpers.setValue(option.value)}
        />
        <ErrorMessage
          component="div"
          className="errorMessage"
          name={props.name}
        />
      </div>
    );
  };

  const MyTextAreaInput = ({ ...props }) => {
    const [, , helpers] = useField(props);
    return (
      <>
        <label className="checkbox form-check-label">{props.label}</label>
        <CKEditor
          className="cke_rtl"
          editor={ClassicEditor}
          data={initialData.ShortDescription}
          config={{
            toolbar: [
              "|",
              "bold",
              "italic",
              "numberedList",
              "bulletedList",
              "|",
              "undo",
              "redo",
            ],
            removePlugins: ["Heading", "Link"],
            language: "fa",
          }}
          onBlur={(_, editor) => {
            const data = editor.getData();
            helpers.setValue(data);
          }}
        />
        <ErrorMessage
          component="div"
          className="errorMessage"
          name={props.name}
        />
      </>
    );
  };

  return (
    <>
      {loading && <MiniSpinner />}
      <Formik
        initialValues={initialData}
        validationSchema={EditProfileEmployerVal}
        onSubmit={(values) => {
          console.log(values)
          submitHandler(values);
        }}
        enableReinitialize={true}
      >
        <section className="complete-register-form container-fluid spx-2 spx-lg-10 smy-10 spt-10">
          <div className="row">
            <aside className="col-12 col-lg-5 mx-auto">
              <Form className="w-100">
                <div className="bg-white srounded-md sp-2 smb-2">

                  <h1 className="fs-l c-dark d-block text-center smb-5 ir-bl">
                    ???????????? ?????????????? ????????
                  </h1>


                  {/* Managment firstName   */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ?????? ?? ?????? ???????????????? ???????????? ???????? ???? ???????? ????????
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="ManagmentFullName"
                        className="form-control ir-r shadow-none"
                        placeholder="?????? ?? ?????? ???????????????? ???????????? ????????"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="ManagmentFullName"
                      />
                    </div>
                  </div>

                  {/* PersianFullName  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ?????? ???????? ???? ???? ?????????? ???????? ????????
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="PersianFullName"
                        className="form-control ir-r shadow-none"
                        placeholder="?????? ???????? ???? ??????????"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="PersianFullName"
                      />
                    </div>
                  </div>

                  {/* EngFullName  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ?????? ???????? ???? ???? ?????????????? ???????? ????????
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="EngFullName"
                        className="form-control ir-r shadow-none"
                        placeholder="?????? ???????? ???? ??????????????"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="EngFullName"
                      />
                    </div>
                  </div>

                  {/* Image  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ?????????? ???????? ?????? ???? ???????? ????????
                    </label>
                    <div className="form-group mb-0">

                      <div className="m-0 p-0 d-flex flex-row justify-content-between align-items-start">
                        <label className="uploadPic" htmlFor="Image ">
                          <i className=" fas fa-camera "></i> ???????????????? ??????
                        </label>
                        <input
                          id="Image "
                          type="file"
                          onChange={(event) => {
                            preview(event.currentTarget.files);
                          }}
                          className="form-control d-none"
                        />


                        {initialData.Image ? (
                          <img
                            src={initialData.Image}
                            style={{
                              width: "80px",
                              height: "80px",
                              objectFit: "cover",
                            }}
                            alt="?????????? ????????"
                          />
                        ) : (
                          <img
                            src={EmployersDefaultLogo}
                            style={{
                              width: "80px",
                              height: "80px",
                              objectFit: "cover",

                            }}
                            alt="?????????? ?????? ??????"
                          />
                        )}




                        {/* {profilePicUrl ? (
                          <img
                            src={profilePicUrl}
                            style={{
                              width: "80px",
                              height: "80px",
                              objectFit: "cover",
                            }}
                            alt="?????????? ????????"
                          />
                        ) : (
                          initialData.Image && (
                            <img
                              src={EmployersDefaultLogo}
                              style={{
                                width: "80px",
                                height: "80px",
                                objectFit: "cover",

                              }}
                              alt="?????????? ?????? ??????"
                            />
                          )
                        )} */}
                      </div>


                      {imageError && (
                        <div className="errorMessage">{imageError}</div>
                      )}
                    </div>
                  </div>

                  {/* Email  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ?????????? ???????? ???? ???????? ????????
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="Email"
                        className="form-control ir-r shadow-none"
                        placeholder="?????????? ????????"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="Email"
                      />
                    </div>
                  </div>

                  {/* EmergencPhone  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ?????????? ???????? ???????? ???? ???????? ????????
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="EmergencPhone"
                        className="form-control ir-r shadow-none"
                        placeholder="?????????? ???????? ????????"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="EmergencPhone"
                      />
                    </div>
                  </div>

                  {/* url */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ???????????? ???????? ???? ???????? ????????
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="url"
                        className="form-control ir-r shadow-none"
                        placeholder="???????????? ????????"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="url"
                      />
                    </div>
                  </div>

                  {/* FieldOfActivity */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ???????? ???????????? ????????
                    </label>
                    <MultiMySelect
                      name="FieldOfActivity"
                      placeholder="???????? ???????????? ?????? ???? ???????????? ????????"
                      options={fieldOptions}
                    />
                  </div>

                  {/* NumberOfStaff */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ?????????? ??????????
                    </label>
                    <MySelect
                      name="NumberOfStaff"
                      placeholder="?????????? ?????????? ???????? ???? ???????????? ????????"
                      options={[
                        { value: 1, label: "?????? 2 ???? 10 ??????" },
                        { value: 2, label: "?????? 11 ???? 50 ??????" },
                        { value: 3, label: "?????? 51 ???? 200 ??????" },
                        { value: 4, label: "?????? 201 ???? 500 ??????" },
                        { value: 5, label: "?????? 501 ???? 1000 ??????" },
                        { value: 6, label: "?????????? ???? 1000 ??????" },
                      ]}
                    />
                  </div>

                  {/* City  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      ?????? ???????? ???????? ???? ???????? ????????
                    </label>
                    <MySelect
                      name="City"
                      placeholder="?????? ???? ???????????? ????????"
                      options={cities}
                    />
                  </div>

                  {/* ShortDescription  */}
                  <div className="col-12 smb-2 ir-r">
                    <MyTextAreaInput
                      label="?????????????? ????????"
                      name="ShortDescription"
                    />
                  </div>

                  {/* Submit Button  */}
                  <div className="smt-3 col-12 d-flex flex-row justify-content-between align-items-center">

                    <button
                      type="submit"
                      className="btn btn-success ir-r spx-4">
                      ??????????????????
                    </button>

                    <button onClick={() => { history.goBack(); }} type="button" className="m-0  btn btn-secondary">
                      ????????????
                    </button>

                  </div>

                </div>
              </Form>
            </aside>
          </div>
        </section>
      </Formik>
    </>
  );
};

export { EditProfileEmployer };
