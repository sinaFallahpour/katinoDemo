import React, { Component } from "react";
import { Link } from "react-router-dom";
import Select from "react-select";

export class JobSearchBox2 extends Component {
  state = {
    selectedOption: null,
    city: "",
    key: "",
    code: "",
  };

  changeHandler = (event) =>
    this.setState({ [event.target.name]: event.target.value });

  cityHandler = (event) => {
    console.log(event)
    this.setState({ city: event.value });
  };

  searchUrl = () => {





    var url = ""

    if (this.state.city && this.state.key && this.state.code) {
      url = `Jobs?key=${this.state.key}&city=${this.state.city}&code=${this.state.code}`
    }
    else if (this.state.city && this.state.key)
      url = `Jobs?key=${this.state.key}&city=${this.state.city}`
    else if (this.state.city && this.state.code)
      url = `Jobs?city=${this.state.city}&code=${this.state.code}`
    else if (this.state.key && this.state.code)
      url = `Jobs?key=${this.state.key}&code=${this.state.code}`

    else if (this.state.key && this.state.city)
      url = `Jobs?key=${this.state.key}&city=${this.state.city}`

    else if (this.state.key)
      url = `Jobs?key=${this.state.key}`
    else if (this.state.city)
      url = `Jobs?city=${this.state.city}`
    else if (this.state.code)
      url = `Jobs?code=${this.state.code}`

    return url;

  }

  codeChangeHandler = (e) => {
    let val = e.target.value;

    if (val.match(/[۰]/g)) {
      val = val.replace(/[۰]/g, '0');
    }
    if (val.match(/[۱]/g)) {
      val = val.replace(/[۱]/g, '1');
    }
    if (val.match(/[۲]/g)) {
      val = val.replace(/[۲]/g, '2');
    }
    if (val.match(/[۳]/g)) {
      val = val.replace(/[۳]/g, '3');
    }
    if (val.match(/[۴]/g)) {
      val = val.replace(/[۴]/g, '4');
    }
    if (val.match(/[۵]/g)) {
      val = val.replace(/[۵]/g, '5');
    }
    if (val.match(/[۶]/g)) {
      val = val.replace(/[۶]/g, '6');
    }
    if (val.match(/[۷]/g)) {
      val = val.replace(/[۷]/g, '7');
    }
    if (val.match(/[۸]/g)) {
      val = val.replace(/[۸]/g, '8');
    }
    if (val.match(/[۹]/g)) {
      val = val.replace(/[۹]/g, '9');
    }
    if (val.match(/[^0123456789]/g)) {
      val = val.replace(/[^0123456789]/g, '');
    }

    this.setState({ [e.target.name]: e.target.value });

  }


  render() {
    let cities = [];

    this.props.cities.map((id) => {
      cities.push({
        value: id.cityDivisionCode,
        label: ` ${id.provinceName}، ${id.cityName} `,
      });
    });

    cities.sort((a, b) => a.label.localeCompare(b.label));

    return (
      <div
        className="row w-100 sp-2  bg-white rounded-content srounded-md"
        style={{ margin: 0 }}
      >
        {/* What? */}
        <div className="col-12  smb-2 pr-lg-0 mb-lg-0" style={{ padding: 0 }}>
          <div className="form-group mb-0 ir-r srounded-md">
            <label className="fs-regular ir-b c-dark">
              به دنبال چه چیزی هستید؟
            </label>

            <input
              onChange={this.changeHandler}
              name="key"
              className="form-control ir-r"
              placeholder="عنوان شغلی، شرکت و..."
            />
          </div>
        </div>


        <div className="col-12  smb-2 pr-lg-0 mb-lg-0 mt-2" style={{ padding: 0 }}>
          <div className="form-group mb-0 ir-r srounded-md">
            <label className="fs-regular ir-b c-dark">
              کد آگهی
            </label>

            <input
              onChange={this.codeChangeHandler}
              name="code"
              className="form-control ir-r"
              placeholder="کدآگهی"
            />
          </div>
        </div>



        {/* Where? */}
        <div
          className="col-12  smb-2 pr-lg-0 mb-lg-0 mt-3"
          style={{ padding: 0 }}
        >
          <div className="form-group mb-0 ir-r srounded-md">
            <label className="fs-regular ir-b c-dark">در کدام شهر؟</label>
            <Select
              value={cities.filter(
                (option) => option.value.toString() === (this.state.city.toString())
              )}
              onChange={(e) => {
                // console.log(e)
                // this.handleFilterSearch(e);
                this.setState({ city: e?.value ? e.value : "" });
              }}
              
              placeholder="انتخاب شهر"
              styles={{ fontFamily: "iransans-regular" }}
              options={cities}
              style={{ color: "#555" }}
            />
          </div>
        </div>

        {/* Search Button */}
        <div
          className="col-12  smb-2 pr-lg-0 mb-lg-0 mt-3"
          style={{ padding: 0 }}
        >
          <Link
            type="button"
            className="btn btn-primary d-block w-100 ir-r fs-m srounded-sm"
            to={
              this.searchUrl()

              // this.state.city && this.state.key
              //   ? `Jobs?key=${this.state.key}&city=${this.state.city}`
              //   : this.state.key
              //     ? `Jobs?key=${this.state.key}`
              //     : this.state.city
              //       ? `Jobs?city=${this.state.city}`
              //       : `Jobs`

            }
          >
            جستجو
          </Link>
        </div>
      </div>
    );
  }
}
