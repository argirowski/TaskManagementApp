import React, { useEffect } from "react";
import { Container, Row, Col, Button, Card } from "react-bootstrap";
import LoaderComponent from "../components/common/LoaderComponent";
import { useNavigate } from "react-router-dom";
import { hasToken } from "../utils/auth";

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = React.useState(false);

  // Redirect authenticated users to projects page
  useEffect(() => {
    if (hasToken()) {
      navigate("/projects", { replace: true });
    }
  }, [navigate]);

  const handleNavigate = (path: string) => {
    setLoading(true);
    setTimeout(() => {
      navigate(path);
    }, 700); // 700ms for visible loader, adjust as needed
  };

  // Don't show home page if user is authenticated (redirect will happen)
  if (hasToken()) {
    return <LoaderComponent message="Loading..." />;
  }

  if (loading) {
    return <LoaderComponent message="Loading..." />;
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} md={8} lg={6} className="mx-auto">
          <Card className="shadow-lg border-0">
            <Card.Body className="p-5">
              <div className="text-center mb-4">
                <h1 className="main-title mb-3">Task Management App</h1>
                <p className="subtitle mb-4">
                  Organize your tasks, boost your productivity, and achieve your
                  goals with our intuitive task management platform.
                </p>
              </div>

              <div className="d-grid gap-3">
                <Button
                  size="lg"
                  onClick={() => handleNavigate("/login")}
                  className="btn-login py-3"
                  disabled={loading}
                >
                  <i className="fas fa-sign-in-alt me-2"></i>
                  Login to Your Account
                </Button>

                <Button
                  size="lg"
                  onClick={() => handleNavigate("/register")}
                  className="btn-register py-3"
                  disabled={loading}
                >
                  <i className="fas fa-user-plus me-2"></i>
                  Create New Account
                </Button>
              </div>

              <div className="text-center mt-4">
                <p className="home-page-footer-text">
                  Join thousands of users who trust us with their task
                  management needs
                </p>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default HomePage;
