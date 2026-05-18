#include <stdio.h>
#include <mpi.h>

// Function prototype for trap function
double Trap(double a, double b, int n, double h);

// Function prototype for user input
void GetInput(int my_rank, int commsize, double* a_p, double* b_p, int* n_p);

// Function prototype for f(x)
double f(double x);

// Main function
int main(void) {
  int my_rank, commsize;
  double a, b;
  int n;
  double h;
  double local_a, local_b;
  int local_n;
  double local_area, total_area;

  // For MPI
  MPI_Init(NULL, NULL);
  MPI_Comm_size(MPI_COMM_WORLD, &commsize);
  MPI_Comm_rank(MPI_COMM_WORLD, &my_rank);

  // Get user input
  GetInput(my_rank, commsize, &a, &b, &n);

  // Calculate the width of a single trapezoid
  h = (b - a) / n;

  // Then calculate this process's local a, b, and n
  local_n = n / commsize;
  local_a = a + my_rank * local_n * h;
  local_b = local_a + local_n * h;

  // Part 3: Add leftover trapezoids to the last process
  if (my_rank == commsize - 1) {
    int remainder = n % commsize;
    local_n += remainder;
    local_b = local_a + local_n * h;
  }

  // Calculate this process's local area
  local_area = Trap(local_a, local_b, local_n, h);

  // Print out this process's local area
  printf("Process %d's area is %f.\n", my_rank, local_area);

  // Part 2: Reduce local areas into total_area on process 0
  MPI_Reduce(&local_area, &total_area, 1, MPI_DOUBLE, MPI_SUM, 0, MPI_COMM_WORLD);

  if (my_rank == 0) {
    printf("The total area is: %f\n", total_area);
  }

  // End of program
  MPI_Finalize();
  return 0;
}

// Function implementation for user input
void GetInput(int my_rank, int commsize, double* a_p, double* b_p, int* n_p) {

  if (my_rank == 0) {
    printf("Enter a, b, and n\n");

    // Use scanf on Ubuntu/Linux
    scanf("%lf %lf %d", a_p, b_p, n_p);
  }

  // Part 1: Broadcast input values to all processes
  MPI_Bcast(a_p, 1, MPI_DOUBLE, 0, MPI_COMM_WORLD);
  MPI_Bcast(b_p, 1, MPI_DOUBLE, 0, MPI_COMM_WORLD);
  MPI_Bcast(n_p, 1, MPI_INT, 0, MPI_COMM_WORLD);
}

// Function implementation for trapezoid rule
double Trap(double a, double b, int n, double h) {

  double area, x;
  int i;

  area = (f(a) + f(b)) / 2.0;
  for (i = 1; i < n; i++) {
    x = a + i * h;
    area += f(x);
  }

  area = area * h;
  return area;
}

// Function implementation for f(x)
double f(double x) {
  return x * x;
}
