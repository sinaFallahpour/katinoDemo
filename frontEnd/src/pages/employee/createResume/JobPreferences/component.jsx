import React, { useState, useEffect } from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';
import { EditMode, SeeMode } from './components';
import { getUserJobPreferenceForCurrentUser, editUserJobPreference, addUserJobPreference } from "./API_SERVICES";
import "./style/style.css";

const JobPreferences = ({getResomePercent}) => {


    const [loading, set_loading] = useState(true);
    const [jobPreferences, set_jobPreferences] = useState(null);
    const [editMode, set_editMode] = useState(false);

    useEffect(() => {

        const getData = async () => {
            await apiCall_seeInfo();
            set_loading(false);
        };
        getData();

    }, []);

    const handleStopLoading = () => {
        set_loading(false);
    };

    const handleStartLoading = () => {
        set_loading(true);
    };

    const apiCall_seeInfo = async () => {
        const data = await getUserJobPreferenceForCurrentUser();
        set_loading(false);
        // console.log(data);
        if (data?.resul) {
            set_jobPreferences(data?.resul);
        }
        else {
            const initialState = {
                "id": null,
                "city": "تهران ، اسلامشهر",
                "typeOfCooperation": 1,
                "senioritylevel": 1,
                "salary": 1,
                "promotion": false,
                "insurance": false,
                "educationCourses": false,
                "flexibleWorkingTime": false,
                "hasMeel": false,
                "transportationService": false,
                "categoryIds": [],
            };
            set_jobPreferences(initialState);
        }
    };

    const apiCall_editInfo = async (data) => {
        const resp = await editUserJobPreference(data);
        return resp;
    };

    const apiCall_addInfo = async (data) => {
        const resp = await addUserJobPreference(data);
        return resp;
    };

    const handleClick_editModeBtn = async () => {

        set_loading(true);

        if (editMode) {
            await apiCall_seeInfo();
            set_editMode(false);
        }
        else {
            set_editMode(true);
        }

    };

    return (
        <section className="my-3 position-relative bg-white p-3 d-flex flex-column justify-content-start align-items-stretch">
            <div style={{ width: "100%", height: "100%", position: "absolute", top: "0", right: "0", zIndex: 50, background: "rgba(48,48,48,0.5)" }}
                className={loading ? "d-flex flex-row justify-content-center align-items-center" : "d-none flex-row justify-content-center align-items-center"}>
                <CircularProgress />
            </div>
            <header className="m-0 mb-2 p-0 d-flex flex-row justify-content-end align-items-center">
                <button onClick={handleClick_editModeBtn}
                    type="button"
                    className="btn btn-info ir-r">
                    {editMode ? "بازگشت" : "ویرایش"}
                </button>
            </header>

            {editMode
                ? <EditMode getResomePercent={getResomePercent} apiCall_addInfo={apiCall_addInfo} handleStopLoading={() => { set_loading(false) }} handleStartLoading={handleStartLoading} apiCall_seeInfo={apiCall_seeInfo} apiCall_editInfo={apiCall_editInfo} jobPreferences={jobPreferences} />
                : <SeeMode handleStopLoading={() => { set_loading(false) }} handleStartLoading={handleStartLoading} jobPreferences={jobPreferences} />
            }
        </section>
    );
}

export default JobPreferences;