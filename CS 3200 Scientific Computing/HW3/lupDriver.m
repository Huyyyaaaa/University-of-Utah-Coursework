clear;
clc;

%% set N for matrix size and nrhs for number of right-hand sides
N = 3;
nrhs =2;
%% get random matrix of N x N
A = rand(N,N);
%A = [2 2 3; 1 1 5; 3 7 2;];
B = rand(N, nrhs); 
%B= [2; 3; 4];
X = zeros(N, nrhs);

%% get L, U, P of LU decomposition
[L,U,P] = lup(A);

%% solve with forward and back sub
for j = 1:nrhs
  X(:,j)=bsub(U,fsub(L, P*B(:,j))); 
end
%% solve using Matlab A\B
X2 = A\B;
%% print relative error
relative_error = norm(X2-X)/norm(X2);
fprintf('relative error: %d\n',relative_error);