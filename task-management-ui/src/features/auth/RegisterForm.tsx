import React, { useState, useEffect } from "react";
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
import { Formik, FormikHelpers } from "formik";
import { AxiosError } from "axios";
import api from "../../api/axios";
import { hasToken } from "../../utils/auth";
import { API_CONFIG } from "../../config/api";
import AlertComponent from "../../components/common/AlertComponent";
import ConfirmDialogComponent from "../../components/common/ConfirmDialogComponent";
import { RegisterFormData, ApiErrorResponse } from "../../types/types";
import { registerUserValidationSchema } from "../../utils/validation";
import LoaderComponent from "../../components/common/LoaderComponent";

const RegisterForm: React.FC = () => {
  const navigate = useNavigate();

  // Redirect authenticated users to projects page
  useEffect(() => {
    if (hasToken()) {
      navigate("/projects", { replace: true });
    }
  }, [navigate]);
  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );
  // Modal state for cancel confirmation
  const [showConfirm, setShowConfirm] = useState(false);

  const initialValues: RegisterFormData = {
    UserName: "",
    UserEmail: "",
    Password: "",
    confirmPassword: "",
  };

  const handleSubmit = async (
    values: RegisterFormData,
    { setSubmitting, setFieldError }: FormikHelpers<RegisterFormData>
  ) => {
    try {
      // Remove confirmPassword from the data sent to API
      const { confirmPassword, ...registrationData } = values;

      // API endpoint for registration
      await api.post(API_CONFIG.ENDPOINTS.AUTH.REGISTER, registrationData);

      // Redirect to login immediately
      navigate("/login");
    } catch (error) {
      // Type-safe error handling
      if (error instanceof AxiosError) {
        const errorData = error.response?.data as ApiErrorResponse | undefined;

        if (errorData?.error) {
          setAlertMessage(errorData.error);
        } else if (errorData?.message) {
          setAlertMessage(errorData.message);
        } else if (errorData?.errors) {
          // Handle validation errors from backend
          Object.keys(errorData.errors).forEach((field) => {
            const fieldErrors = errorData.errors![field];
            if (Array.isArray(fieldErrors) && fieldErrors.length > 0) {
              setFieldError(field, fieldErrors[0]);
            }
          });
          setAlertMessage("Please fix the errors below");
        } else {
          setAlertMessage("Registration failed. Please try again.");
        }
      } else {
        // Handle non-Axios errors
        const errorMessage =
          error instanceof Error
            ? error.message
            : "Registration failed. Please try again.";
        setAlertMessage(errorMessage);
      }

      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setSubmitting(false);
    }
  };

  // Don't show register form if user is authenticated (redirect will happen)
  if (hasToken()) {
    return <LoaderComponent message="Loading..." />;
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} xl={5} className="mx-auto">
          <Card className="custom-card border-0">
            <Card.Header className="text-center custom-card-header py-4">
              <h4 className="header-title mb-0">Create Your Account</h4>
            </Card.Header>
            <Card.Body className="p-4">
              <AlertComponent
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              <Formik
                initialValues={initialValues}
                validationSchema={registerUserValidationSchema}
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
                  dirty,
                }) => {
                  // Handler for Back to Home, now inside Formik
                  const handleBackToHome = () => {
                    if (dirty) {
                      setShowConfirm(true);
                    } else {
                      navigate(-1);
                    }
                  };
                  return (
                    <>
                      <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group className="mb-3" controlId="formUsername">
                          <Form.Label className="login-register-form-label">
                            Username
                          </Form.Label>
                          <Form.Control
                            type="text"
                            name="UserName"
                            placeholder="Enter username"
                            value={values.UserName}
                            onChange={handleChange}
                            onBlur={handleBlur}
                            isInvalid={touched.UserName && !!errors.UserName}
                          />
                          <Form.Control.Feedback type="invalid">
                            {errors.UserName}
                          </Form.Control.Feedback>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formEmail">
                          <Form.Label className="login-register-form-label">
                            Email address
                          </Form.Label>
                          <Form.Control
                            type="email"
                            name="UserEmail"
                            placeholder="Enter email"
                            value={values.UserEmail}
                            onChange={handleChange}
                            onBlur={handleBlur}
                            isInvalid={touched.UserEmail && !!errors.UserEmail}
                          />
                          <Form.Control.Feedback type="invalid">
                            {errors.UserEmail}
                          </Form.Control.Feedback>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formPassword">
                          <Form.Label className="login-register-form-label">
                            Password
                          </Form.Label>
                          <Form.Control
                            type="password"
                            name="Password"
                            placeholder="Enter password"
                            value={values.Password}
                            onChange={handleChange}
                            onBlur={handleBlur}
                            isInvalid={touched.Password && !!errors.Password}
                          />
                          <Form.Control.Feedback type="invalid">
                            {errors.Password}
                          </Form.Control.Feedback>
                        </Form.Group>

                        <Form.Group
                          className="mb-3"
                          controlId="formConfirmPassword"
                        >
                          <Form.Label className="login-register-form-label">
                            Confirm Password
                          </Form.Label>
                          <Form.Control
                            type="password"
                            name="confirmPassword"
                            placeholder="Confirm password"
                            value={values.confirmPassword}
                            onChange={handleChange}
                            onBlur={handleBlur}
                            isInvalid={
                              touched.confirmPassword &&
                              !!errors.confirmPassword
                            }
                          />
                          <Form.Control.Feedback type="invalid">
                            {errors.confirmPassword}
                          </Form.Control.Feedback>
                        </Form.Group>

                        <div className="d-grid gap-2">
                          <Button
                            type="submit"
                            size="lg"
                            disabled={isSubmitting}
                            className="btn-create-account"
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
                                Creating Account...
                              </>
                            ) : (
                              "Create Account"
                            )}
                          </Button>
                          <Button
                            onClick={handleBackToHome}
                            disabled={isSubmitting}
                            size="lg"
                            className="btn-back-home"
                          >
                            Back to Home
                          </Button>
                        </div>
                      </Form>
                      <ConfirmDialogComponent
                        show={showConfirm}
                        title="Cancel Registration?"
                        message="Are you sure you want to cancel? Your changes will be lost."
                        confirmText="Yes"
                        cancelText="No"
                        onConfirm={() => {
                          setShowConfirm(false);
                          navigate(-1);
                        }}
                        onCancel={() => setShowConfirm(false)}
                        variant="danger"
                      />
                    </>
                  );
                }}
              </Formik>
            </Card.Body>
            <Card.Footer className="text-center custom-card-footer py-3">
              <p className="footer-unified">
                Already have an account?
                <Button
                  onClick={() => navigate("/login")}
                  className="signin-link"
                >
                  Sign in here
                </Button>
              </p>
            </Card.Footer>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default RegisterForm;
