import React from "react";
import { Refrence } from "./Refrence";

export function Refrences(props) {
    return (
        <div className="row companies smr-2 sml-2 position-relative">
            {props.refrences &&
                props.refrences.map((item, index) => (
                    <div key={index} className="col-12 p-0 c-item">
                        <Refrence
                            refrence={item}
                            fullName={item.fullName}
                            address={item.address}
                            phoneNUmber={item.phoneNUmber}
                        // name={item.companyPersianName}
                        // website={item.url}
                        // logo={item.image}
                        // city={item.city}
                        // rate={item.rate}
                        // enName={item.companyEngName}
                        // description={item.description}
                        // hasLink={true}

                        />
                    </div>
                ))}
        </div>
    );
}
