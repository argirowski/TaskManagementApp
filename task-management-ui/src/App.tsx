import React from "react";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import store from "./store/store";
import AuthInitializer from "./components/auth/AuthInitializer";
import Navigation from "./components/layout/Navigation";
import AppRoutes from "./routes/AppRoutes";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <AuthInitializer>
        <BrowserRouter>
          <Navigation />
          <div className="task-management-app">
            <AppRoutes />
          </div>
        </BrowserRouter>
      </AuthInitializer>
    </Provider>
  );
};

export default App;
