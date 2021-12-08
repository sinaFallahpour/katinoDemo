import React, { useEffect, useState } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import { useHistory } from "react-router-dom";
import { Formik, Field, Form, ErrorMessage, useField } from "formik";
import Select from "react-select";

import { CompleteRegister } from "../../core/validation/completeRegister";
import API_ADDRESS from "../../API_ADDRESS";
import parse from 'react-html-parser';
import { MiniSpinner } from "../../components/spinner/MiniSpinner";
import EmployersDefaultLogo from './EmployersDefaultLogo.jpg';
import "./Field.style.css";

const CompleteProfile = () => {
  const initialData = {
    PersianFullName: "",
    EngFullName: "",
    EmergencPhone: "",
    Image: "",
    url: "",
    FieldOfActivity: "",
    NumberOfStaff: "",
    Email: "",
    ManagmentFullName: "",
    City: "",
  };
  const [fieldOptions, setFieldOptions] = useState([]);
  const [profilePicUrl, setProfilePicUrl] = useState("");
  const [PicUrl, setPicUrl] = useState("");
  const [imageError, setImageError] = useState("");
  const [selectedValue, setSelectedValue] = useState([]);
  const [loading, setLoading] = useState(false);
  const [cities, setCities] = useState([]);

  const history = useHistory();

  useEffect(() => {
    // let categories
    const getData = async () => {

      /////////////////////////
      /////// CATEGORIES //////////
      /////////////////////////

      let fieldOptions = [];
      const circle_symbol = parse('&#9679');
      const dash_symbol = parse('&#8211');
      const space_symbol = parse("&nbsp;");

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
        // console.log(fieldOptions)
        setFieldOptions(fieldOptions);
      });

      /////////////////////////
      /////// CITIES //////////
      /////////////////////////

      let cities = [];
      await axios.get(API_ADDRESS + "Account/GetCities").then((res) => {
        res.data.resul.map((item) => {
          cities.push({
            value: item.cityDivisionCode,
            label: `${item.provinceName}، ${item.cityName}`,
          });
        });
        setCities(cities);
        setLoading(false);
      });
    };
    getData();


  }, []);

  const convertObjToFormData = (obj) => {
    const fd = new FormData();
    for (let key in obj) {
      if (key !== "FieldOfActivity") {
        fd.append(key, obj[key]);
      }
    }

    console.log(obj)
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
      setImageError("فرمت درست را وارد کنید");
      return;
    }

    setPicUrl(files[0]);
    var reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = () => {
      setProfilePicUrl(reader.result);
    };
  };

  const submitHandler = (values) => {
    console.log(values)
    if (values.FieldOfActivity.length === 0) {
      toast.error("لطفا حوزه فعالیت خود را وارد نمایید .");
      return;
    }
    if (values.FieldOfActivity.url === 0) {
      toast.error("لطفا آدرس وبسایت خود را وارد نمایید .");
      return;
    }
    setLoading(true);
    let tempo = { ...values, Image: PicUrl };

    const correctFormat = convertObjToFormData(tempo);
    axios
      .post(API_ADDRESS + "Account/CompanySubmitRegistrstion", correctFormat, {
        headers: {
          Authorization: `bearer ${localStorage.getItem("JWT")}`,
        },
      })
      .then((res) => {
        if (res.status === 200) {
          history.push("/Employer/Dashboard");
        }
        Swal.fire({
          icon: "success",
          title: "تکمیل فرم با موفقیت انجام شد",
          showConfirmButton: false,
          timer: 1750,
        });
        setLoading(false);
      })
      .catch((err) => {
        setLoading(false);

        if (err.response.status === 400 && err.response) {
          err.response.data.message.map((e) => {
            toast.error(e);
          });
        }
        err.response.data.message?.map((e) => {
          toast.error(e);
        });
        setLoading(false);
      });
  };

  const MultiMySelect = ({ label, options, ...props }) => {
    const [field, meta, helpers] = useField(props);

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
    const [field, meta, helpers] = useField(props);
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

  return (
    <>
      {loading && MiniSpinner()}
      <Formik
        initialValues={initialData}
        validationSchema={CompleteRegister}
        onSubmit={(values) => {
          submitHandler(values);
        }}
      >
        <section className="complete-register-form container-fluid spx-2 spx-lg-10 smy-10 spt-10">
          <div className="row">
            <aside className="col-12 col-lg-5 mx-auto">
              <Form className="w-100">
                <div className="bg-white srounded-md sp-2 smb-2">
                  <h1 className="fs-l c-dark d-block text-center smb-5 ir-bl">
                    تکمیل پروفایل شرکت
                  </h1>

                  {/* ManagmentFullName   */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      لطفا نام و نام خانوادگی مدیریت شرکت وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="ManagmentFullName"
                        className="form-control ir-r shadow-none"
                        placeholder="نام و نام خانوادگی مدیریت شرکت"
                        type="text"
                      />
                      <ErrorMessage
                        component="div"
                        className="errorMessage"
                        name="ManagmentFullName"
                      />
                    </div>
                  </div>

                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      لطفا نام شرکت را به فارسی وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="PersianFullName"
                        className="form-control ir-r shadow-none"
                        placeholder="نام شرکت به فارسی"
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
                      لطفا نام شرکت را به انگلیسی وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="EngFullName"
                        className="form-control ir-r shadow-none"
                        placeholder="نام شرکت به انگلیسی"
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
                      لطفا لوگوی شرکت خود را وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <label className="uploadPic" htmlFor="Image ">
                        <i className=" fas fa-camera "></i> بارگزاری عکس
                      </label>
                      <input
                        id="Image "
                        type="file"
                        onChange={(event) => {
                          preview(event.currentTarget.files);
                        }}
                        className="form-control"
                      />
                      {profilePicUrl ? (
                        <img
                          src={profilePicUrl}
                          style={{
                            width: "80px",
                            height: "80px",
                            objectFit: "cover",
                          }}
                          alt="لوگوی شرکت"
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
                            alt="لوگوی پیش فرض"
                          />
                        )
                      )}
                      {imageError && (
                        <div className="errorMessage">{imageError}</div>
                      )}
                    </div>
                  </div>

                  {/* Email  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      لطفا ایمیل شرکت را وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="Email"
                        className="form-control ir-r shadow-none"
                        placeholder="ایمیل شرکت"
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
                      لطفا شماره تماس شرکت را وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="EmergencPhone"
                        className="form-control ir-r shadow-none"
                        placeholder="شماره تماس شرکت"
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
                      لطفا وبسایت شرکت را وارد کنید
                    </label>
                    <div className="form-group mb-0">
                      <Field
                        name="url"
                        className="form-control ir-r shadow-none"
                        placeholder="وبسایت شرکت"
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
                      حوزه فعالیت شرکت
                    </label>
                    <MultiMySelect
                      name="FieldOfActivity"
                      placeholder="حوزه فعالیت خود را انتخاب کنید"
                      options={fieldOptions}
                    />
                  </div>

                  {/* NumberOfStaff */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      تعداد پرسنل
                    </label>
                    <MySelect
                      name="NumberOfStaff"
                      placeholder="محدوده تعداد پرنسل شرکت را انتخاب کنید"
                      options={[
                        { value: 1, label: "بین 2 تا 10 نفر" },
                        { value: 2, label: "بین 11 تا 50 نفر" },
                        { value: 3, label: "بین 51 تا 200 نفر" },
                        { value: 4, label: "بین 201 تا 500 نفر" },
                        { value: 5, label: "بین 501 تا 1000 نفر" },
                        { value: 6, label: "بیشتر از 1000 نفر" },
                      ]}
                    />
                  </div>
                  {/* City  */}
                  <div className="col-12 smb-2">
                    <label className="ir-r d-block text-right smb-1">
                      شهر مقرر شرکت را وارد کنید
                    </label>
                    <MySelect
                      name="City"
                      placeholder="شهر را انتخاب کنید"
                      options={cities}
                    />
                  </div>
                  {/* submit button  */}
                  <div className="smt-3 col-12">
                    <button
                      type="submit"
                      className="btn btn-warning ir-r spx-4"
                    >
                      تایید
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

export { CompleteProfile };
