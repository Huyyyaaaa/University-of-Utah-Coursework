clear;
clc;

%% set N for matrix size and nrhs for number of right-hand sides
N = 100;
nrhs = 50;
%% get random matrix of N x N
A = rand(N,N);
B = rand(N, nrhs); 
X = zeros(N, nrhs);

elapsed1 = zeros(nrhs);
tic; %start timer
%% get L, U, P of LU decomposition
[L,U,P] = lup(A);

%% solve with forward and back sub
for j = 1:nrhs
  X(:,j)=bsub(U,fsub(L, P*B(:,j))); 
  elapsed1(j) = toc; %stop timer
end


%% setup for second timing test
elapsed2 = zeros(nrhs);
tic; %start timer
for j=1:nrhs
   [L,U,P] = lup(A);
   X(:,j)=bsub(U,fsub(L, P*B(:,j))); 
   elapsed2(j) = toc;%stop timer 2
end

%% plot graphs for timing
figure;
loglog(1:nrhs, elapsed1);
hold on;
loglog(1:nrhs, elapsed2);
legend("A single LUP calculation for each RHS", "one LUP calculation for every RHS");
