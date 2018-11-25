import React, { Component } from "react";
import "./app.css";

import Login from "./components/login";

class App extends Component {
  render() {
    return (
      <div className="App">
        <header className="App-header">
          <Login />
        </header>
      </div>
    );
  }
}

export default App;
