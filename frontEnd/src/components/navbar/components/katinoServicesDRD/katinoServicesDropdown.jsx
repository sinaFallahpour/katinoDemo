import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import './style.css';


const KatinoServicesDropdown = (props) => {

    const history = useHistory();
    const [data, set_data] = useState(null);
    const [open, set_open] = useState(false);

    useEffect(() => {

        const getData = async () => {
            const url = "https://panel.katinojob.ir/api/Services/List";
            try {
                const resp = await axios.get(url);
                if (resp.data.resul) {
                    set_data(resp.data.resul);
                }
            } catch (ex) {
                if (ex.response) {
                    console.log(ex.response);
                }
            }
        };

        getData();
        // set_data(testServices);

    }, []);

    const handleOpen = () => {
        set_open(true);
    };

    const handleClose = () => {
        set_open(false);
    };

    const testServices = [
        {
            id: '0', title: "خدمات 0", children: [
                { id: '00', title: "خدمات 1_1", children: [] },
                { id: '01', title: "خدمات 1_2", children: [] },
                { id: '02', title: "خدمات 1_3", children: [] },
            ]
        },
        {
            id: '1', title: "خدمات 1", children: [
                { id: '10', title: "خدمات 1_1", children: [] },
                { id: '11', title: "خدمات 1_2", children: [] },
                { id: '22', title: "خدمات 1_3", children: [] },
            ]
        },
    ];

    return (
        <>
            <div className="m-0 p-0 katino-services-dropdown">
                <div onClick={() => { set_open(!open) }} className="ksd__btn m-0 p-0 d-flex flex-row justify-content-center align-items-center text-nowrap">
                    <span style={{ color: props.color ? props.color : "#fff" }} className="m-0 p-0 ir-r">خدمات</span>
                    <i style={{ color: props.color ? props.color : "#fff" }} className="m-0 mr-1 p-0 fa fa-angle-down"></i>
                </div>
                {/* {open && */}
                <div id="ksd__list" className={open ? "show-ksd__list ksd__list list-level-0 m-0 p-0" : " ksd__list list-level-0 m-0 p-0"}>
                    <div className="m-0 p-0 inner">
                        <ul className="m-0 p-0 list-unstyled list-level-0__ul d-flex flex-column justify-content-start align-items-stretch">
                            {data && data.map(item => (
                                <>
                                    {item.children.length === 0 &&
                                        <li onClick={e => history.push(`/khadamatMa?id=${item.id}`)} key={item.id} className="m-0 p-2 list-level-0__ul__li">
                                            <Link onClick={() => { set_open(false) }} to={`/khadamatMa?id=${item.id}`} className="m-0 p-2 w-100 h100 text-decoration-none ir-r  zzzz">{item.title}</Link>
                                        </li>
                                    }
                                    {item.children.length > 0 &&
                                        <li key={item.id} className="m-0 p-2 list-level-0__ul__li">
                                            <Link onClick={() => { set_open(false) }} to={`/khadamatMa?id=${item.id}`} className="m-0 p-0 text-decoration-none ir-r 22222">{item.title}</Link>
                                            <i className="fa fa-angle-left mr-1"></i>
                                            <div className="m-0 p-0 list-level-1">
                                                <ul className=" list-unstyled m-0 p-0 list-level-1__ul  d-flex flex-column justify-content-start align-items-stretch">
                                                    {item.children.map(item_child => (
                                                        <li key={item.id + "_" + item_child.id} className="m-0 p-2 list-level-1__ul__li">
                                                            <Link onClick={() => { set_open(false) }} to={`/khadamatMa?id=${item_child.id}`} className="m-0 p-0 text-decoration-none ir-r">{item_child.title}</Link>
                                                        </li>
                                                    ))}
                                                </ul>
                                            </div>
                                        </li>
                                    }
                                </>
                            ))}

                        </ul>
                    </div>
                </div>
                {/* } */}

            </div>
            {open && <div onClick={() => { set_open(false) }} style={{
                zIndex: "900",
                position: "fixed",
                top:'0',
                right:"0",
                width: "100vw",
                height: "100vh",
                backgroundColor: "rgba(0,0,0,0)"
            }}
            ></div>}
        </>
    );
}

export default KatinoServicesDropdown;