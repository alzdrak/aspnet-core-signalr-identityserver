import React, { Component } from "react";

const NotifierContext = React.createContext();

export class NotifierProvider extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isOpen: false,
      message: ""
    };
  }

  openNotifier = message => {
    this.setState({
      message,
      isOpen: true
    });
  };

  closeNotifier = () => {
    this.setState({
      message: "",
      isOpen: false
    });
  };

  render() {
    const { children } = this.props;

    return (
      <NotifierContext.Provider
        value={{
          openNotifier: this.openNotifier,
          closeNotifier: this.closeNotifier,
          NotifierIsOpen: this.state.isOpen,
          message: this.state.message
        }}
      >
        {/* TODO: Render Notifier presentation component here */}
        {children}
      </NotifierContext.Provider>
    );
  }
}

export const NotifierConsumer = NotifierContext.Consumer;
