clear; 
clc;
close all;

%% Define the unknown function to interpolate
f = @(x) sin(x) + 0.1*rand(length(x),1);

%% Numbers of data points.
Nd = 5;

%% polynomial degree L
L = 4;

%% Setup the points at which we want to evaluate the interpolant
xe = linspace(-1,1,1000)'; %generate evenly-spaced points
ye = f(xe); %evaluate function

%% Start plotting stuff
figure
plot(xe,ye,'black');
%pause %waits for input

x = linspace(-1,1,Nd)'; %generate evenly-spaced points in [-1,1].
y = f(x);

%% Form Vandermonde matrix "intelligently"
V = zeros(Nd, L);
for vit = 1:L
    V(:,vit) = x.^(vit-1);
end

%% Compute coefficients by QR decomposition.
[Q,R] = qr(V);
a=R\(Q*y);

%% Evaluate polynomial interpolant by building eval matrix and
%% multiplying with coefficients.
Ve = zeros(length(xe),L);
for vit = 1:L
    Ve(:,vit) = xe.^(vit-1);
end
ye_num = Ve*a; %Do the matrix-vector product to predict values

%% Plot stuff
hold on;
plot(xe,ye_num,'-');
drawnow
%pause




    
    