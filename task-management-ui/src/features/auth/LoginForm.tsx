import React, { useEffect, useState } from "react";
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
import { setToken, hasToken, setTokenData } from "../../utils/auth";
import { API_CONFIG } from "../../config/api";
import AlertComponent from "../../components/common/AlertComponent";
import ConfirmDialogComponent from "../../components/common/ConfirmDialogComponent";
import {
  LoginFormData,
  ApiErrorResponse,
  LoginResponse,
} from "../../types/types";
import { loginUserValidationSchema } from "../../utils/validation";
import LoaderComponent from "../../components/common/LoaderComponent";

const LoginForm: React.FC = () => {
  const navigate = useNavigate();

  // Local error state (removed Redux error handling)
  const [error, setError] = useState<string | null>(null);
  // Modal state for cancel confirmation
  const [showConfirm, setShowConfirm] = useState(false);

  // Redirect authenticated users to projects page
  useEffect(() => {
    if (hasToken()) {
      navigate("/projects", { replace: true });
    }
  }, [navigate]);

  // Clear local error on unmount
  useEffect(() => {
    return () => {
      setError(null);
    };
  }, []);

  const initialValues: LoginFormData = {
    UserEmail: "",
    Password: "",
  };

  const handleSubmit = async (
    values: LoginFormData,
    { setSubmitting }: FormikHelpers<LoginFormData>
  ) => {
    try {
      setError(null);

      // Perform login request directly (stateless). Save token to localStorage.
      const res = await api.post<LoginResponse>(
        API_CONFIG.ENDPOINTS.AUTH.LOGIN,
        values
      );

      // Extract all token data from response (handling both camelCase and PascalCase)
      const responseData = res.data;
      const accessToken =
        responseData?.accessToken || responseData?.AccessToken;
      const refreshToken =
        responseData?.refreshToken || responseData?.RefreshToken;
      const userName =
        responseData?.userName ||
        responseData?.UserName ||
        responseData?.name ||
        responseData?.username;

      if (accessToken && refreshToken && userName) {
        setTokenData({
          accessToken,
          refreshToken,
          userName,
        });
      } else {
        // Fallback: if response structure is different, try to save what we can
        if (accessToken) {
          setToken(accessToken);
        }
        if (userName) {
          localStorage.setItem("userName", userName);
        }
      }

      // On success, navigate to projects
      navigate("/projects");
    } catch (error) {
      // Type-safe error handling
      if (error instanceof AxiosError) {
        const errorData = error.response?.data as
          | ApiErrorResponse
          | string
          | undefined;

        if (typeof errorData === "string") {
          setError(errorData);
        } else if (errorData && typeof errorData === "object") {
          const message =
            errorData.error ||
            errorData.message ||
            "Login failed. Please try again.";
          setError(message);
        } else {
          setError("Login failed. Please try again.");
        }
      } else {
        // Handle non-Axios errors
        const errorMessage =
          error instanceof Error
            ? error.message
            : "Login failed. Please try again.";
        setError(errorMessage);
      }
    } finally {
      setSubmitting(false);
    }
  };

  // Don't show login form if user is authenticated (redirect will happen)
  if (hasToken()) {
    return <LoaderComponent message="Loading..." />;
  }

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
              <AlertComponent
                show={!!error}
                variant="danger"
                message={error || ""}
                onClose={() => setError(null)}
              />

              <Formik
                initialValues={initialValues}
                validationSchema={loginUserValidationSchema}
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
                      <ConfirmDialogComponent
                        show={showConfirm}
                        title="Cancel Login?"
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
