import {
  projectValidationSchema,
  tasksValidationSchema,
  registerUserValidationSchema,
  loginUserValidationSchema,
} from "../utils/validation";

describe("Validation Schemas", () => {
  describe("projectValidationSchema", () => {
    it("should validate a valid project", async () => {
      const validProject = {
        projectName: "My Project",
        projectDescription: "This is a valid project description",
      };

      await expect(
        projectValidationSchema.validate(validProject)
      ).resolves.toEqual(validProject);
    });

    it("should reject project with missing name", async () => {
      const invalidProject = {
        projectDescription: "Some description",
      };

      await expect(
        projectValidationSchema.validate(invalidProject)
      ).rejects.toThrow("Project name is required");
    });

    it("should reject project name shorter than 3 characters", async () => {
      const invalidProject = {
        projectName: "AB",
        projectDescription: "Some description",
      };

      await expect(
        projectValidationSchema.validate(invalidProject)
      ).rejects.toThrow("Project name must be at least 3 characters");
    });

    it("should reject project name longer than 50 characters", async () => {
      const invalidProject = {
        projectName: "A".repeat(51),
        projectDescription: "Some description",
      };

      await expect(
        projectValidationSchema.validate(invalidProject)
      ).rejects.toThrow("Project name must be less than 50 characters");
    });

    it("should reject description shorter than 10 characters when provided", async () => {
      const invalidProject = {
        projectName: "My Project",
        projectDescription: "Short",
      };

      await expect(
        projectValidationSchema.validate(invalidProject)
      ).rejects.toThrow("Description must be at least 10 characters");
    });

    it("should reject description longer than 200 characters", async () => {
      const invalidProject = {
        projectName: "My Project",
        projectDescription: "A".repeat(201),
      };

      await expect(
        projectValidationSchema.validate(invalidProject)
      ).rejects.toThrow("Description must be less than 200 characters");
    });

    it("should accept description exactly 10 characters", async () => {
      const validProject = {
        projectName: "My Project",
        projectDescription: "A".repeat(10),
      };

      await expect(
        projectValidationSchema.validate(validProject)
      ).resolves.toEqual(validProject);
    });

    it("should accept description exactly 200 characters", async () => {
      const validProject = {
        projectName: "My Project",
        projectDescription: "A".repeat(200),
      };

      await expect(
        projectValidationSchema.validate(validProject)
      ).resolves.toEqual(validProject);
    });
  });

  describe("tasksValidationSchema", () => {
    it("should validate a valid task", async () => {
      const validTask = {
        projectTaskTitle: "My Task",
        projectTaskDescription: "This is a valid task description",
      };

      await expect(tasksValidationSchema.validate(validTask)).resolves.toEqual(
        validTask
      );
    });

    it("should reject task with missing title", async () => {
      const invalidTask = {
        projectTaskDescription: "Some description",
      };

      await expect(tasksValidationSchema.validate(invalidTask)).rejects.toThrow(
        "Task title is required"
      );
    });

    it("should reject task with missing description", async () => {
      const invalidTask = {
        projectTaskTitle: "My Task",
      };

      await expect(tasksValidationSchema.validate(invalidTask)).rejects.toThrow(
        "Description is required"
      );
    });

    it("should reject task title shorter than 3 characters", async () => {
      const invalidTask = {
        projectTaskTitle: "AB",
        projectTaskDescription: "Some description",
      };

      await expect(tasksValidationSchema.validate(invalidTask)).rejects.toThrow(
        "Task title must be at least 3 characters"
      );
    });

    it("should reject task title longer than 100 characters", async () => {
      const invalidTask = {
        projectTaskTitle: "A".repeat(101),
        projectTaskDescription: "Some description",
      };

      await expect(tasksValidationSchema.validate(invalidTask)).rejects.toThrow(
        "Task title must be less than 100 characters"
      );
    });

    it("should reject description shorter than 10 characters", async () => {
      const invalidTask = {
        projectTaskTitle: "My Task",
        projectTaskDescription: "Short",
      };

      await expect(tasksValidationSchema.validate(invalidTask)).rejects.toThrow(
        "Description must be at least 10 characters"
      );
    });

    it("should reject description longer than 500 characters", async () => {
      const invalidTask = {
        projectTaskTitle: "My Task",
        projectTaskDescription: "A".repeat(501),
      };

      await expect(tasksValidationSchema.validate(invalidTask)).rejects.toThrow(
        "Description must be less than 500 characters"
      );
    });
  });

  describe("registerUserValidationSchema", () => {
    it("should validate a valid user registration", async () => {
      const validUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "Password123",
        confirmPassword: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(validUser)
      ).resolves.toEqual(validUser);
    });

    it("should reject missing username", async () => {
      const invalidUser = {
        UserEmail: "john@example.com",
        Password: "Password123",
        confirmPassword: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Username is required");
    });

    it("should reject username shorter than 3 characters", async () => {
      const invalidUser = {
        UserName: "Jo",
        UserEmail: "john@example.com",
        Password: "Password123",
        confirmPassword: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Username must be at least 3 characters");
    });

    it("should reject username longer than 20 characters", async () => {
      const invalidUser = {
        UserName: "A".repeat(21),
        UserEmail: "john@example.com",
        Password: "Password123",
        confirmPassword: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Username must be less than 20 characters");
    });

    it("should reject invalid email format", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "invalid-email",
        Password: "Password123",
        confirmPassword: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Invalid email address");
    });

    it("should reject email longer than 100 characters", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: `${"a".repeat(95)}@example.com`,
        Password: "Password123",
        confirmPassword: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Email must be less than 100 characters");
    });

    it("should reject password shorter than 6 characters", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "Pass1",
        confirmPassword: "Pass1",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Password must be at least 6 characters");
    });

    it("should reject password longer than 50 characters", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "A".repeat(51),
        confirmPassword: "A".repeat(51),
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Password must be less than 50 characters");
    });

    it("should reject password without uppercase letter", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "password123",
        confirmPassword: "password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow(
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      );
    });

    it("should reject password without lowercase letter", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "PASSWORD123",
        confirmPassword: "PASSWORD123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow(
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      );
    });

    it("should reject password without number", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "Password",
        confirmPassword: "Password",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow(
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      );
    });

    it("should reject non-matching confirm password", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "Password123",
        confirmPassword: "Different123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Passwords must match");
    });

    it("should reject missing confirm password", async () => {
      const invalidUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "Password123",
      };

      await expect(
        registerUserValidationSchema.validate(invalidUser)
      ).rejects.toThrow("Confirm password is required");
    });

    it("should accept valid password with all requirements", async () => {
      const validUser = {
        UserName: "john_doe",
        UserEmail: "john@example.com",
        Password: "MyP@ssw0rd",
        confirmPassword: "MyP@ssw0rd",
      };

      await expect(
        registerUserValidationSchema.validate(validUser)
      ).resolves.toEqual(validUser);
    });
  });

  describe("loginUserValidationSchema", () => {
    it("should validate a valid login", async () => {
      const validLogin = {
        UserEmail: "john@example.com",
        Password: "Password123",
      };

      await expect(
        loginUserValidationSchema.validate(validLogin)
      ).resolves.toEqual(validLogin);
    });

    it("should reject missing email", async () => {
      const invalidLogin = {
        Password: "Password123",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow("Email is required");
    });

    it("should reject invalid email format", async () => {
      const invalidLogin = {
        UserEmail: "invalid-email",
        Password: "Password123",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow("Invalid email address");
    });

    it("should reject email longer than 100 characters", async () => {
      const invalidLogin = {
        UserEmail: `${"a".repeat(95)}@example.com`,
        Password: "Password123",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow("Email must be less than 100 characters");
    });

    it("should reject missing password", async () => {
      const invalidLogin = {
        UserEmail: "john@example.com",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow("Password is required");
    });

    it("should reject password shorter than 6 characters", async () => {
      const invalidLogin = {
        UserEmail: "john@example.com",
        Password: "Pass1",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow("Password must be at least 6 characters");
    });

    it("should reject password longer than 50 characters", async () => {
      const invalidLogin = {
        UserEmail: "john@example.com",
        Password: "A".repeat(51),
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow("Password must be less than 50 characters");
    });

    it("should reject password without uppercase letter", async () => {
      const invalidLogin = {
        UserEmail: "john@example.com",
        Password: "password123",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow(
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      );
    });

    it("should reject password without lowercase letter", async () => {
      const invalidLogin = {
        UserEmail: "john@example.com",
        Password: "PASSWORD123",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow(
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      );
    });

    it("should reject password without number", async () => {
      const invalidLogin = {
        UserEmail: "john@example.com",
        Password: "Password",
      };

      await expect(
        loginUserValidationSchema.validate(invalidLogin)
      ).rejects.toThrow(
        "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      );
    });
  });
});
