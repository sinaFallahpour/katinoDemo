// import { useEffect } from "react";
// import { withRouter } from "react-router-dom";

// export const ScrollToTop = ({ children, location: { pathname } }) => {
//   useEffect(() => {
//     window.scrollTo(0, 0);
//   }, [pathname]);

//   return children || null;
// };

// export default withRouter(ScrollToTop);


import { useEffect } from 'react';
import { withRouter } from 'react-router-dom';

function ScrollToTop({ history }) {
  document.getElementById("root").scrollIntoView();
}

export default withRouter(ScrollToTop);