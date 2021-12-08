import React, { useState, useEffect } from 'react';
import { translate_jobPreferenceItemValue, translate_jobPreferenceItemKey } from "../../translateEnumValues";

const JobPreferenceItemSingleValue = (props) => {
    return (
        <div className="m-0 my-2 p-0 ir-r d-flex flex-row justify-content-start align-items-baseline">
            <span style={{ color: "#666" }} className="m-0 ml-2 p-0 text-nowrap" >
                {`${props.title} : `}
            </span>
            <span style={{ fontWeight: "600" }} className="m-0 p-0" >
                {props.value}
            </span>
        </div>
    );
}

const JobPreferenceItemMultiValue = (props) => {
    return (
        <div className="m-0 my-2 p-0 ir-r d-flex flex-row justify-content-start align-items-baseline">
            <span style={{ color: "#666" }} className="m-0 ml-2 p-0 text-nowrap" >
                {`${props.title} : `}

            </span>
            <ul className="m-0 p-0 list-unstyled d-flex flex-row flex-wrap justify-content-start align-items-baseline">
                {props.children && props.children.map(item =>
                    <li style={{ fontWeight: "600" }} key={item.categoryId} id={'categoryId_' + item.categoryId} className="m-0 mr-1 mt-1 text-white rounded bg-success p-2">
                        {item.categoryName}
                    </li>
                )}
            </ul>
        </div>
    );
};

const SeeMode = ({ jobPreferences = null, handleStopLoading }) => {

    useEffect(() => {
        if (jobPreferences) {
            handleStopLoading();

        }
    }, [jobPreferences]);

    return (
        <div className="m-0 p-0 d-flex flex-column justify-content-start align-items-stretch">
            {jobPreferences && Object.keys(jobPreferences).map(item => {

                // console.log(jobPreferences[item])

                if (item === "categoryIds" || item === "id") {
                    return '';
                }
                if (Array.isArray(jobPreferences[item])) {
                    return (
                        <JobPreferenceItemMultiValue title={translate_jobPreferenceItemKey(item)}>
                            {jobPreferences[item]}
                        </JobPreferenceItemMultiValue>
                    );
                }
                return (
                    <JobPreferenceItemSingleValue title={translate_jobPreferenceItemKey(item)} value={translate_jobPreferenceItemValue(jobPreferences[item], item)} />
                );

            })}
        </div>
    );
};

export default SeeMode;