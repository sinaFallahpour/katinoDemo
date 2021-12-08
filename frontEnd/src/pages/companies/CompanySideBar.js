import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./companyDetails.styles.css";
import { makeStyles } from '@material-ui/core/styles';
import Modal from '@material-ui/core/Modal';
import Backdrop from '@material-ui/core/Backdrop';
import { useSpring, animated } from 'react-spring'; // web.cjs is required for IE 11 support
import API_ADDRESS, { API_URL } from "../../API_ADDRESS";


const useStyles = makeStyles((theme) => ({
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
  paper: {
    backgroundColor: theme.palette.background.paper,
    border: '2px solid #000',
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
}));

const Fade = React.forwardRef(function Fade(props, ref) {
  const { in: open, children, onEnter, onExited, ...other } = props;
  const style = useSpring({
    from: { opacity: 0 },
    to: { opacity: open ? 1 : 0 },
    onStart: () => {
      if (open && onEnter) {
        onEnter();
      }
    },
    onRest: () => {
      if (!open && onExited) {
        onExited();
      }
    },
  });

  return (
    <animated.div ref={ref} style={style} {...other}>
      {children}
    </animated.div>
  );
});


const enum_numberOfStaff = [
  { value: 1, label: "بین 2 تا 10 نفر" },
  { value: 2, label: "بین 11 تا 50 نفر" },
  { value: 3, label: "بین 51 تا 200 نفر" },
  { value: 4, label: "بین 201 تا 500 نفر" },
  { value: 5, label: "بین 501 تا 1000 نفر" },
  { value: 6, label: "بیشتر از 1000 نفر" },
];

const get_staffNumber = (value) => {
  let label = "";
  enum_numberOfStaff.every(item => {
    if (item.value.toString() === value.toString()) {
      label = item.label;
      return false;
    }
    return true;
  })
  return label;
};


const CompanySideBar = (props) => {

  const classes = useStyles();
  const [open, setOpen] = React.useState(false);
  const [imgSrc, set_imgSrc] = React.useState("");

  const handleClose = () => {
    setOpen(false);
  };
  const handleToggle = () => {
    setOpen(!open);
  };



  console.log(props?.gallery)
  return (
    <div className="sideBarHolder m-0 p-3 d-flex flex-column justify-content-start align-items-stretch" >

      <div className="">

        <div className="my-2">
          <i className="fa fa-user"></i>{" "}
          {props.managementFullName ? props.managementFullName : "---"}
        </div>

        <a href={"mailto:" + props.email} className="my-2 text-decoration-none">
          <i className="fa fa-envelope"></i> {props.email ? props.email : "---"}
        </a>

        <a href={"https://" + props?.website} className=" d-block my-2 text-decoration-none">
          <i className="fa fa-globe"></i> {props.website ? props.website : "---"}
        </a>

        <a href={"tel:" + props?.phoneNumber} className="my-2 text-decoration-none">
          <i className="fa fa-phone"></i>{" "}
          {props.phoneNumber ? props.phoneNumber : "---"}
        </a>

        <div className="my-2">
          <i className="fa fa-users"></i>{" "}
          {props.numberOfStaff ? get_staffNumber(props.numberOfStaff) : "---"}
        </div>

        <div className="my-2">
          {props.isActive ? (
            <span className="c-success">
              <i className="far fa-thumbs-up"></i> فعال
            </span>
          ) : (
            <span className="c-danger">
              <i className="far fa-thumbs-down"></i> غیرفعال
            </span>
          )}
        </div>
      </div>




      <div className="my-3 d-flex flex-row flex-wrap justify-content-start align-items-stretch">
        {props?.gallery?.map((item, index) => {
          console.log(item)
          return (
            < img onClick={() => {
              set_imgSrc(`${API_URL.slice(0, -1)}${item}`);
              handleToggle();
            }}
              alt={props.name}
              style={{ cursor: "zoom-in" }}
              className="col-6 m-0 p-1"
              src={`${API_URL.slice(0, -1)}${item}`} />
          )
        })}
      </div>

      <Modal
        aria-labelledby="spring-modal-title"
        aria-describedby="spring-modal-description"
        className={classes.modal}
        open={open}
        onClose={handleClose}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
      >
        <Fade in={open}>
          <div className={classes.paper}>
            <img className="mx-auto" style={{ cursor: "zoom-out", zIndex: "1000", position: "absolute", top: "100px", right: "0", left: "0", maxWidth: "90vw", maxHeight: "90vh", minWidth: "375px", minHeight: "300px" }} src={imgSrc} alt="تصویر" />


          </div>
        </Fade>
      </Modal>


    </div>
  );

}

export { CompanySideBar };
