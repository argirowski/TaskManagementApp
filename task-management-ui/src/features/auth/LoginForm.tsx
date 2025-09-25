import React, { useState } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  Form,
  Button,
  Spinner,
} from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { Formik } from "formik";
import * as Yup from "yup";
import axios from "axios";
import Alert from "../../components/common/Alert";
import "./auth.css";

interface LoginFormData {
  email: string;
  password: string;
}

// Validation schema using Yup
const validationSchema = Yup.object({
  email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters")
    .required("Password is required"),
});

const LoginForm: React.FC = () => {
  const navigate = useNavigate();
  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );

  const initialValues: LoginFormData = {
    email: "",
    password: "",
  };

  const handleSubmit = async (
    values: LoginFormData,
    { setSubmitting, setFieldError }: any
  ) => {
    try {
      // API endpoint for login
      const response = await axios.post(
        "https://localhost:7272/api/Auth/login",
        values
      );

      console.log("Login successful:", response.data);

      // Store auth token if your API returns one
      if (response.data.token) {
        localStorage.setItem("authToken", response.data.token);
      }

      // Redirect immediately on successful login
      navigate("/dashboard"); // Change this to your desired route after login
    } catch (error: any) {
      console.error("Login error:", error);

      if (error.response?.data?.message) {
        setAlertMessage(error.response.data.message);
      } else if (error.response?.data?.errors) {
        // Handle validation errors from backend
        Object.keys(error.response.data.errors).forEach((field) => {
          setFieldError(field, error.response.data.errors[field][0]);
        });
        setAlertMessage("Please fix the errors below");
      } else if (error.response?.status === 401) {
        setAlertMessage("Invalid email or password");
      } else {
        setAlertMessage("Login failed. Please try again.");
      }

      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setSubmitting(false);
    }
  };

  const handleBackToHome = () => {
    navigate("/");
  };

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} xl={4} className="mx-auto">
          <Card className="custom-card border-0">
            <Card.Header className="text-center custom-card-header py-4">
              <h4 className="header-title mb-0">Sign In to Your Account</h4>
            </Card.Header>
            <Card.Body className="p-4">
              <Alert
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              <Formik
                initialValues={initialValues}
                validationSchema={validationSchema}
                onSubmit={handleSubmit}
              >
                {({
                  values,
                  errors,
                  touched,
                  handleChange,
                  handleBlur,
                  handleSubmit,
                  isSubmitting,
                }) => (
                  <Form noValidate onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="formEmail">
                      <Form.Label className="form-label">
                        Email address
                      </Form.Label>
                      <Form.Control
                        type="email"
                        name="email"
                        placeholder="Enter email"
                        value={values.email}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        isInvalid={touched.email && !!errors.email}
                      />
                      <Form.Control.Feedback type="invalid">
                        {errors.email}
                      </Form.Control.Feedback>
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formPassword">
                      <Form.Label className="form-label">Password</Form.Label>
                      <Form.Control
                        type="password"
                        name="password"
                        placeholder="Enter password"
                        value={values.password}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        isInvalid={touched.password && !!errors.password}
                      />
                      <Form.Control.Feedback type="invalid">
                        {errors.password}
                      </Form.Control.Feedback>
                    </Form.Group>

                    <Form.Group
                      className="form-label mb-3"
                      controlId="formRememberMe"
                    >
                      <Form.Check type="checkbox" label="Remember me" />
                    </Form.Group>

                    <div className="d-grid gap-2">
                      <Button
                        className="btn-sign-in"
                        type="submit"
                        size="lg"
                        disabled={isSubmitting}
                      >
                        {isSubmitting ? (
                          <>
                            <Spinner
                              as="span"
                              animation="border"
                              size="sm"
                              role="status"
                              className="me-2"
                            />
                            Signing In...
                          </>
                        ) : (
                          "Sign In"
                        )}
                      </Button>
                      <Button
                        className="btn-back-home"
                        size="lg"
                        onClick={handleBackToHome}
                        disabled={isSubmitting}
                      >
                        Back to Home
                      </Button>
                    </div>
                  </Form>
                )}
              </Formik>
            </Card.Body>
            <Card.Footer className="text-center custom-card-footer">
              <p className="footer-unified">
                Don't have an account?
                <Button
                  onClick={() => navigate("/register")}
                  className="signin-link"
                >
                  Create account here
                </Button>
              </p>
            </Card.Footer>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default LoginForm;
