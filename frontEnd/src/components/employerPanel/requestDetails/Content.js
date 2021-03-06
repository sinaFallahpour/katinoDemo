import React, { Component } from "react";
import { SendedResume } from "./SendedResume";
import { Profile } from "./Profile";

export class Content extends Component {
  state = {
    sendedResume: true,
  };

  tabsHandler = () => {
    this.state.sendedResume === true
      ? this.setState({ sendedResume: false })
      : this.setState({ sendedResume: true });
  };

  render() {
    return (
      <div id="contentHolder" className="bg-white sbs-shadow srounded-md sp-2">
        <div className="request-details-intro">
          <div className="detail d-flex justify-content-start align-items-center smb-3">
            <img
              className="sml-1 srounded-sm"
              height="80"
              src="/img/index-blog/01.png"
            />

            <div>
              <span className="d-block ir-b c-dark text-right fs-m smb-1">
                {this.props.allInfo.userPersonalInfoForResome?.fullName || "-"}
                {" - "}
                {this.props.allInfo.userPersonalInfoForResome?.jobTitle || "-"}
              </span>
              <span className="d-block ir-r c-regular text-right fs-m mb-0">
                تاریخ ارسال رزومه: 1399/07/01 12:56
              </span>
            </div>
          </div>

          <ul className="tabs m-0 p-0 d-flex align-items-center justify-content-start">
            <li
              onClick={this.tabsHandler}
              className={`tabs-item cursor-pointer spy-1 spx-2 ir-r ${
                this.state.sendedResume ? "active" : ""
              }`}
            >
              رزومه ارسال شده
            </li>

            <li
              onClick={this.tabsHandler}
              className={`tabs-item cursor-pointer spy-1 spx-2 ir-r ${
                !this.state.sendedResume ? "active" : ""
              }`}
            >
              پروفایل کارجو
            </li>
          </ul>
        </div>

        {this.state.sendedResume ? (
          <SendedResume
            userShortInfoForResome={this.props.userShortInfoForResome}
          />
        ) : (
          <Profile allInfo={this.props.allInfo} />
        )}
      </div>
    );
  }
}
