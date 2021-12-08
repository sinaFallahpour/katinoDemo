import React from "react";
import parse from 'html-react-parser';

import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import ADDRESS from "../../ADDRESS";
import ReactHtmlParser from "react-html-parser";
import "./company.style.css";


//mui packages
import { makeStyles } from '@material-ui/core/styles';
import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Modal from '@material-ui/core/Modal';
import Backdrop from '@material-ui/core/Backdrop';
import { useSpring, animated } from 'react-spring'; // web.cjs is required for IE 11 support

// Import Swiper React components
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/swiper.min.css';
import API_ADDRESS, { API_URL } from "../../API_ADDRESS";





const useStyles = makeStyles((theme) => ({
  root: {
    width: '100%',
  },
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


export function Refrence(props) {
  const classes = useStyles();

  const linkCheck = () => {
    if (!props.enName) {
      toast.error("شرکت مورد نظر صفحه ای ندارد");
    }
  };

  const [open, setOpen] = React.useState(false);
  const [imgSrc, set_imgSrc] = React.useState("");

  const handleClose = () => {
    setOpen(false);
  };
  const handleToggle = () => {
    setOpen(!open);
  };


  return (
    <Accordion style={props?.refrence?.isMain === true ? {border:"3px solid #FFD700",borderRadius:"1.5rem"} : {}} dir="rtl">
      <AccordionSummary
        aria-controls="panel1a-content"
        id="panel1a-header"
      >
        <div

          className="w-100 cartContainer bg-white srounded-md"
        >
          <header className="cartHeaderContainer">

            <h4 style={{ fontWeight: props.refrence.isMain ? "600" : "400" }} className="w-100 text-right">
              {props?.refrence?.isMain === true && <i className="fa fa-1x fa-check-circle ml-1"></i>}
              {props.fullName} </h4>


            <div className="options d-flex justify-content-start justify-content-lg-end align-items-center smt-2 mt-lg-0">

              {props.phoneNUmber}

            </div>
          </header>

          <div className="cartContextContainer">
            <div>
              <h6> <i className="fa fa-building"></i> {props.address ? props.address : "---"}</h6>
            </div>
            <div>

            </div>

          </div>

        </div>
      </AccordionSummary>
      <AccordionDetails>
        <div className="w-100 m-0 p-0 reference-more-detail d-flex flex-column justify-content-start align-items-stretch">
          <div style={{ boxShadow: "0px 3px 5px 0px #ebebeb" }} className="m-0 p-0 rmd__gallery_iframe d-flex flex-row flex-wrap justify-content-start align-items-stretch">
            <div className="m-0 p-2 rmd__gallery col-lg-6 col-12 border">
              
              <div style={{ boxShadow: "0px 3px 5px 0px #ebebeb" }} className="m-0 my-2 p-2 rmd__decription border">
                {props?.refrence?.description && parse(props.refrence.description.toString())}
              </div>

              <div className="my-3 d-flex flex-row flex-wrap justify-content-start align-items-center">
                {props?.refrence?.gallery?.map((item, index) => {
                  console.log(props.refrence.gallery)
                  return (
                    < img onClick={() => { set_imgSrc(`${API_URL.slice(0, -1)}${item}`); handleToggle(); }}
                      alt={props.name}
                      style={{ width: "90px", height: "90px", cursor: "zoom-in" }
                      } className=" m-0 p-1" src={`${API_URL.slice(0, -1)}${item}`} />
                    // str = str.substring(0, str.length - 1);
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
                  timeout: 300,
                }}
              >
                <Fade in={open}>
                  <div className={classes.paper}>
                    <img className="mx-auto" style={{ zIndex: "1000", position: "absolute", top: "100px", right: "0", left: "0", maxWidth: "90vw", maxHeight: "90vh", minWidth: "375px", minHeight: "300px" }} src={imgSrc} alt="تصویر" />
                  </div>
                </Fade>
              </Modal>


            </div>
            <div className="m-0 my-lg-0 my-2 p-2 rmd__ifame col-lg-6 col-12 border">
              {props?.refrence?.iframe && parse(props.refrence.iframe.toString())}
            </div>
          </div>

        </div>
      </AccordionDetails>
    </Accordion>

  );
}

// {props.website ? (
//   <a
//     className="website ir-r c-regular d-flex justify-content-start align-items-center text-decoration-none smr-2"
//     href={`http://${props.website}`}
//     target="_blank"
//   >
//     <i className="fas fa-globe c-regular fs-s sml-1"></i>
//     {props.website}
//   </a>
// ) : (
//   ""
// )}

function rate(num) {
  switch (num) {
    case 1:
      return (
        <div className="rate-stars d-flex flex-row-reverse">
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-grey"></i>
          <i className="fas fa-star c-grey"></i>
          <i className="fas fa-star c-grey"></i>
          <i className="fas fa-star c-grey"></i>
        </div>
      );
      break;

    case 2:
      return (
        <div className="rate-stars d-flex flex-row-reverse">
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-grey"></i>
          <i className="fas fa-star c-grey"></i>
          <i className="fas fa-star c-grey"></i>
        </div>
      );
      break;

    case 3:
      return (
        <div className="rate-stars d-flex flex-row-reverse">
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-grey"></i>
          <i className="fas fa-star c-grey"></i>
        </div>
      );
      break;

    case 4:
      return (
        <div className="rate-stars d-flex flex-row-reverse">
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-grey"></i>
        </div>
      );
      break;

    case 5:
      return (
        <div className="rate-stars d-flex flex-row-reverse">
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
          <i className="fas fa-star c-gold"></i>
        </div>
      );
      break;
  }
}
