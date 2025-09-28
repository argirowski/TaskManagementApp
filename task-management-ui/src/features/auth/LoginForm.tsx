import React, { useEffect } from "react";
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
import { useAppDispatch, useAppSelector } from "../../store/hooks";
import { loginUser, clearAuthError } from "../../store/actions/authActions";
import AlertComponent from "../../components/common/AlertComponent";
import "./auth.css";
import { LoginFormData } from "../../types/types";
import { loginUserValidationSchema } from "../../utils/validation";

const LoginForm: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  // Get auth state from Redux
  const { loading, error, isAuthenticated } = useAppSelector(
    (state) => state.auth
  );

  // Redirect if already authenticated
  useEffect(() => {
    if (isAuthenticated) {
      navigate("/projects");
    }
  }, [isAuthenticated, navigate]);

  // Clear error when component unmounts or when user starts typing
  useEffect(() => {
    return () => {
      if (error) {
        dispatch(clearAuthError());
      }
    };
  }, [error, dispatch]);

  const initialValues: LoginFormData = {
    UserEmail: "",
    Password: "",
  };

  const handleSubmit = async (
    values: LoginFormData,
    { setSubmitting }: any
  ) => {
    try {
      // Clear any previous errors
      if (error) {
        dispatch(clearAuthError());
      }

      // Dispatch login action
      await dispatch(loginUser(values) as any);

      // Navigation will be handled by useEffect when isAuthenticated changes
    } catch (error) {
      // Error handling is managed by Redux action
      console.error("Login submission error:", error);
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
              <AlertComponent
                show={!!error}
                variant="danger"
                message={error || ""}
                onClose={() => dispatch(clearAuthError())}
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
                }) => (
                  <Form noValidate onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="formEmail">
                      <Form.Label className="form-label">
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
                      <Form.Label className="form-label">Password</Form.Label>
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
                        disabled={isSubmitting || loading}
                      >
                        {isSubmitting || loading ? (
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
                        disabled={isSubmitting || loading}
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
