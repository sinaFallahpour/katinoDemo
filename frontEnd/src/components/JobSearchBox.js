import React, { Component } from "react";
import Select from "react-select";
import "./job.styles.css";

export class JobSearchBox extends Component {
  state = {
    selectedOption: null,
    city: "",
    key: "",
    code: "",
  };



  componentWillReceiveProps = (nextProps) => {
    console.log(this.props.InitialUrlValue)
    if (nextProps.InitialUrlValue !== this.props.InitialUrlValue) {
      const listOfData = { ...nextProps.InitialUrlValue };

      for (let key in listOfData) {
        this.setState((prevState) => ({
          ...prevState.data,
          [key]: listOfData[key] ? listOfData[key] : "",
        }));
      }
    }
  };

  changeHandler = (event) => {
    this.setState({ [event.target.name]: event.target.value }, () => { console.log(this.state.key) });
  };

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

  handleFilterSearch = async (e, { action, name }) => {
    if (action === "select-option") {
      await this.setState({
        ...this.state.data,
        [name]: e ? e.value : "",
      });
      this.props.handleFilter(this.state);
    } else if (action === "clear") {
      await this.setState({
        ...this.state.data,
        [name]: null,
      });
      this.props.handleFilter(this.state);
    }
  };

  render() {
    let cities = [];

    this.props.cities.map((id) => {
      cities.push({
        value: id.cityDivisionCode,
        label: ` ${id.provinceName}، ${id.cityName} `,
      });
    });

    return (
      <div className=" jobSearchContainer">
        {/* What? */}
        <div className="col-12 col-lg-3 smb-2 pr-lg-0 mb-lg-0">
          <div className="form-group mb-0 ir-r srounded-md">
            <label className="fs-regular ir-b c-dark">
              به دنبال چه چیزی هستید؟
            </label>

            <input
              value={this.state.key}
              onChange={this.changeHandler}
              name="key"
              className="form-control ir-r"
              placeholder="عنوان شغلی، شرکت و..."
            />
          </div>
        </div>

        <div className="col-12 col-lg-3 smb-2 pr-lg-0 mb-lg-0" >
          <div className="form-group mb-0 ir-r srounded-md">
            <label className="fs-regular ir-b c-dark">
              کد آگهی
            </label>

            <input
              value={this.state.code}
              onChange={this.codeChangeHandler}
              name="code"
              className="form-control ir-r"
              placeholder="کدآگهی"
            />
          </div>
        </div>

        {/* Where? */}
        <div className="col-12 col-lg-3 smb-2 mb-lg-0">
          <div className="form-group mb-0 ir-r srounded-md">
            <label className="fs-regular ir-b c-dark">در کدام شهر؟</label>
            <Select
              isClearable
              value={cities.filter(
                (option) => option.value.toString() === (this.state.city.toString())
              )}
              onChange={(e) => {
                // console.log(e)
                // this.handleFilterSearch(e);
                this.setState({ city: e?.value ? e.value : "" });
              }}
              defaultValue={cities.filter(
                (option) => option.value.toString() === (this.state.city.toString())
              )}
              placeholder="انتخاب شهر"
              styles={{ fontFamily: "iransans-regular" }}
              options={cities}
              name="city"
            />
          </div>
        </div>

        {/* Search Button */}
        <div className="col-12 col-lg-3 mb-0 pl-lg-0 d-flex align-items-end">
          <button
            onClick={() => {
              this.props.handleFilter(this.state);
            }}
            type="button"
            className="btn btn-primary d-block w-100 ir-r fs-m srounded-sm"
            dideo-checked="true"
          >
            جستجو
          </button>
        </div>
      </div >
    );
  }
}
