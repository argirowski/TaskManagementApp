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
import * as Yup from "yup";
import axios from "axios";
import Alert from "../../components/common/Alert";
import Loader from "../../components/common/Loader";
import "./project.css";

interface ProjectFormData {
  projectName: string;
  projectDescription: string;
}

interface Project extends ProjectFormData {
  id: string;
}

// Validation schema using Yup
const validationSchema = Yup.object({
  projectName: Yup.string()
    .min(3, "Project name must be at least 3 characters")
    .max(100, "Project name must be less than 100 characters")
    .required("Project name is required"),
  projectDescription: Yup.string().max(
    500,
    "Description must be less than 500 characters"
  ),
});

const ProjectForm: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEditing = Boolean(id);

  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );
  const [initialLoading, setInitialLoading] = useState(isEditing);

  const initialValues: ProjectFormData = {
    projectName: "",
    projectDescription: "",
  };

  const [formValues, setFormValues] = useState<ProjectFormData>(initialValues);

  useEffect(() => {
    if (isEditing && id) {
      fetchProject(id);
    }
  }, [isEditing, id]);

  const fetchProject = async (projectId: string) => {
    try {
      setInitialLoading(true);
      const response = await axios.get(
        `https://localhost:7272/api/Projects/${projectId}`
      );
      const project: Project = response.data;
      setFormValues({
        projectName: project.projectName,
        projectDescription: project.projectDescription || "",
      });
    } catch (error: any) {
      console.error("Error fetching project:", error);
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
        const response = await axios.put(
          `https://localhost:7272/api/Projects/${id}`,
          values
        );
        console.log("Project updated:", response.data);
        setAlertMessage("Project updated successfully!");
      } else {
        // Create new project
        const response = await axios.post(
          "https://localhost:7272/api/Projects",
          values
        );
        console.log("Project created:", response.data);
        setAlertMessage("Project created successfully!");
      }

      setAlertVariant("success");
      setShowAlert(true);

      // Redirect to projects list after 2 seconds
      setTimeout(() => {
        navigate("/projects");
      }, 2000);
    } catch (error: any) {
      console.error("Project submission error:", error);

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

  const handleCancel = () => {
    navigate("/projects");
  };

  if (initialLoading) {
    return <Loader message="Loading project..." />;
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
              <Alert
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              <Formik
                initialValues={formValues}
                validationSchema={validationSchema}
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
                }) => (
                  <Form noValidate onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="formProjectName">
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
                        isInvalid={touched.projectName && !!errors.projectName}
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
                        Optional. Maximum 500 characters.
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
                        onClick={handleCancel}
                        disabled={isSubmitting}
                      >
                        Cancel
                      </Button>
                    </div>
                  </Form>
                )}
              </Formik>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ProjectForm;
