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
import { useNavigate, useParams } from "react-router-dom";
import { Formik } from "formik";
import {
  fetchProject,
  createProject,
  updateProject,
} from "../../services/projectService";
import AlertComponent from "../../components/common/AlertComponent";
import LoaderComponent from "../../components/common/LoaderComponent";
import { projectValidationSchema } from "../../utils/validation";
import { ProjectFormData } from "../../types/types";
import ConfirmDialogComponent from "../../components/common/ConfirmDialogComponent";

const ProjectForm: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEditing = Boolean(id);

  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );
  // Modal state for cancel confirmation
  const [showConfirm, setShowConfirm] = useState(false);
  const [initialLoading, setInitialLoading] = useState(isEditing);

  const initialValues: ProjectFormData = {
    projectName: "",
    projectDescription: "",
  };

  const [formValues, setFormValues] = useState<ProjectFormData>(initialValues);

  useEffect(() => {
    if (isEditing && id) {
      loadProject(id);
    }
  }, [isEditing, id]);

  const loadProject = async (projectId: string) => {
    try {
      setInitialLoading(true);
      const project = await fetchProject(projectId);
      setFormValues({
        projectName: project.projectName,
        projectDescription: project.projectDescription || "",
      });
    } catch (error: any) {
      setAlertMessage("Failed to load project. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setInitialLoading(false);
    }
  };

  const handleSubmit = async (
    values: ProjectFormData,
    { setSubmitting, setFieldError }: any
  ) => {
    try {
      if (isEditing && id) {
        // Update existing project
        await updateProject(id, values);
      } else {
        // Create new project
        await createProject(values);
      }

      // Redirect to projects list immediately
      navigate("/projects");
    } catch (error: any) {
      if (error.response?.data?.message) {
        setAlertMessage(error.response.data.message);
      } else if (error.response?.data?.errors) {
        // Handle validation errors from backend
        Object.keys(error.response.data.errors).forEach((field) => {
          setFieldError(
            field.toLowerCase(),
            error.response.data.errors[field][0]
          );
        });
        setAlertMessage("Please fix the errors below");
      } else {
        setAlertMessage(
          isEditing
            ? "Failed to update project. Please try again."
            : "Failed to create project. Please try again."
        );
      }

      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setSubmitting(false);
    }
  };

  if (initialLoading) {
    return <LoaderComponent message="Loading project..." />;
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} className="mx-auto">
          <Card className="custom-project-card border-0">
            <Card.Header className="text-center custom-project-card-header py-4">
              <h4 className="project-card-header-title mb-0">
                {isEditing ? "Edit Project" : "Create New Project"}
              </h4>
            </Card.Header>
            <Card.Body className="p-4">
              <AlertComponent
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              <Formik
                initialValues={formValues}
                validationSchema={projectValidationSchema}
                enableReinitialize={true}
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
                      {" "}
                      <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group
                          className="mb-3"
                          controlId="formProjectName"
                        >
                          <Form.Label className="project-form-labe">
                            Project Name *
                          </Form.Label>
                          <Form.Control
                            type="text"
                            name="projectName"
                            placeholder="Enter project name"
                            value={values.projectName}
                            onChange={handleChange}
                            onBlur={handleBlur}
                            isInvalid={
                              touched.projectName && !!errors.projectName
                            }
                          />
                          <Form.Control.Feedback type="invalid">
                            {errors.projectName}
                          </Form.Control.Feedback>
                        </Form.Group>

                        <Form.Group
                          className="mb-3"
                          controlId="formProjectDescription"
                        >
                          <Form.Label className="project-form-label">
                            Project Description
                          </Form.Label>
                          <Form.Control
                            as="textarea"
                            rows={4}
                            name="projectDescription"
                            placeholder="Enter project description (optional)"
                            value={values.projectDescription}
                            onChange={handleChange}
                            onBlur={handleBlur}
                            isInvalid={
                              touched.projectDescription &&
                              !!errors.projectDescription
                            }
                          />
                          <Form.Control.Feedback type="invalid">
                            {errors.projectDescription}
                          </Form.Control.Feedback>
                          <Form.Text className="project-form-footer">
                            Optional. Maximum 200 characters.
                          </Form.Text>
                        </Form.Group>

                        <div className="d-grid gap-2">
                          <Button
                            className="btn-create-project"
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
                                {isEditing ? "Updating..." : "Creating..."}
                              </>
                            ) : isEditing ? (
                              "Update Project"
                            ) : (
                              "Create Project"
                            )}
                          </Button>
                          <Button
                            className="btn-cancel"
                            size="lg"
                            onClick={handleBackToHome}
                            disabled={isSubmitting}
                          >
                            Cancel
                          </Button>
                        </div>
                      </Form>
                      <ConfirmDialogComponent
                        show={showConfirm}
                        title="Cancel Adding a Project?"
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
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ProjectForm;
