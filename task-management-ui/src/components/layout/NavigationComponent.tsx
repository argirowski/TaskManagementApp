import React, { useEffect, useState } from "react";
import { Container, Navbar, Nav, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { clearToken, getUserName } from "../../utils/auth";
import LoaderComponent from "../common/LoaderComponent";

const NavigationComponent: React.FC = () => {
  const navigate = useNavigate();

  const [userName, setUserName] = useState<string | null>(null);
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  useEffect(() => {
    // Get the user name from localStorage using auth utility
    const storedName = getUserName();
    setUserName(storedName);
  }, []);

  const handleLogout = () => {
    setIsLoggingOut(true);
    clearToken();
    // Small delay to show loader before navigation
    setTimeout(() => {
      navigate("/");
    }, 700);
  };

  // Show loader during logout
  if (isLoggingOut) {
    return <LoaderComponent message="Logging out..." />;
  }

  return (
    <Navbar
      bg="dark"
      variant="dark"
      expand="lg"
      fixed="top"
      className="align-items-center"
    >
      <Container>
        <Navbar.Brand href="/projects" className="fs-3 fw-bold">
          Task Management
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="ms-auto align-items-center">
            <Navbar.Text className="me-3 text-white fs-5">
              {userName ? `Welcome, ${userName}!` : "Welcome!"}
            </Navbar.Text>
            <Button variant="dark" size="lg" onClick={handleLogout}>
              Logout
            </Button>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default NavigationComponent;
