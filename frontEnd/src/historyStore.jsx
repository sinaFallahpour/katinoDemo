import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';

const Context = React.createContext();

const BrowserHistoryContext = (props) => {

    const [history, set_History] = useState("ddddd");

    return (
        <Context.Provider value={history}>
            {props.children}
        </Context.Provider>
    );
}

export { BrowserHistoryContext };
export default Context;